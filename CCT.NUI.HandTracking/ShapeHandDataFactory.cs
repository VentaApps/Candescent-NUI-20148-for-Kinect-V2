using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core;
using CCT.NUI.Core.Clustering;
using CCT.NUI.Core.Shape;

namespace CCT.NUI.HandTracking
{
    internal class ShapeHandDataFactory : IHandDataFactory
    {
        private HandDataSourceSettings settings;
        private PalmFinder palmFinder;
        private FingerPointDetector fingerPointDetector;
        private FingerBaseDetector fingerBaseDetector;
        private IdGenerator idGenerator = new IdGenerator();

        private HandCollection currentValue;

        public ShapeHandDataFactory(HandDataSourceSettings settings)
        {
            this.settings = settings;
            this.fingerPointDetector = new FingerPointDetector(settings);
            this.fingerBaseDetector = new FingerBaseDetector(settings);
            this.palmFinder = new PalmFinder(settings);
            this.currentValue = new HandCollection();
        }

        public HandCollection Create(ShapeCollection shapes)
        {
            if (shapes.Count == 0)
            {
                this.currentValue = Clear();
            }
            else
            {
                this.currentValue = CreateHandCollection(shapes);
            }
            return this.currentValue;
        }

        private HandCollection CreateHandCollection(ShapeCollection shapes)
        {
            var map = new DistanceMap<HandData, Shape>(currentValue.Hands);
            map.Map(shapes.Shapes);

            var handData = new List<HandData>();
            foreach (var tupple in map.MappedItems)
            {
                handData.Add(this.Create(tupple.Item1, tupple.Item2));
            }
            foreach (var shape in map.UnmappedItems)
            {
                handData.Add(this.Create(shape));
            }
            foreach (var discontinuedHandData in map.DiscontinuedItems)
            {
                this.ReturnIdToPool(discontinuedHandData.Id);
            }

            return new HandCollection(handData);
        }

        private HandCollection Clear()
        {
            this.ClearIds();
            return new HandCollection();
        }

        private void ReturnIdToPool(int id)
        {
            this.idGenerator.Return(id);
        }

        private HandData Create(Shape shape)
        {
            return this.Create(idGenerator.GetNextId(), shape, new List<FingerPoint>());
        }

        private HandData Create(HandData lastFrameData, Shape shape)
        {
            return this.Create(lastFrameData.Id, shape, lastFrameData.FingerPoints.Union(lastFrameData.NewlyDetectedFingerPoints).ToList());
        }

        private void ClearIds()
        {
            this.idGenerator.Clear();
        }

        private HandData Create(int id, Shape shape, IList<FingerPoint> lastFrameFingerPoints)
        {
            var newFingerPoints = this.DetectFingerPoints(shape.ConvexHull, shape.Contour);
            var fingerPoints = this.MapFingerPoints(lastFrameFingerPoints, newFingerPoints);
            var palm = DetectPalm(shape, shape.Contour);

            if (settings.DetectFingerDirection)
            {
                this.fingerBaseDetector.Detect(shape.Contour, fingerPoints);
            }

            return new HandData(id, shape, palm, fingerPoints.Where(f => f.FrameCount >= this.settings.FramesForNewFingerPoint).ToList()) { NewlyDetectedFingerPoints = fingerPoints.Where(f => f.FrameCount < this.settings.FramesForNewFingerPoint).ToList() };
        }

        private Palm DetectPalm(Shape shape, Contour contour)
        {
            var candidates = shape.Points;
            Palm palm = null;

            if (this.settings.DetectCenterOfPalm && shape.PointCount > 0 && contour.Count > 0)
            {
                palm = this.palmFinder.FindCenter(shape.ConvexHull, contour, shape.Points);
            }
            return palm;
        }

        private IList<FingerPoint> MapFingerPoints(IList<FingerPoint> oldFingerPoints, IList<FingerPoint> newFingerPoints)
        {
            var idGenerator = new IdGenerator();
            var distanceMap = new DistanceMap<FingerPoint, FingerPoint>(oldFingerPoints);
            distanceMap.Map(newFingerPoints);
            foreach (var tuple in distanceMap.MappedItems)
            {
                idGenerator.SetUsed(tuple.Item1.Id);
                tuple.Item2.Id = tuple.Item1.Id;
                tuple.Item2.FrameCount = tuple.Item1.FrameCount + 1;
            }
            foreach (var newFinger in distanceMap.UnmappedItems)
            {
                newFinger.Id = idGenerator.GetNextId();
            }
            foreach (var discontinuedFinger in distanceMap.DiscontinuedItems)
            {
                discontinuedFinger.NegativeFrameCount++;
            }
            return distanceMap.MappedItems.Select(i => i.Item2).Union(distanceMap.UnmappedItems).Union(distanceMap.DiscontinuedItems).Where(i => i.NegativeFrameCount <= this.settings.FramesForDiscontinuedFingerPoint).ToList();
        }

        private IList<FingerPoint> DetectFingerPoints(ConvexHull convexHull, Contour contour)
        {
            if (!this.settings.DetectFingers)
            {
                return new List<FingerPoint>();
            }
            return this.fingerPointDetector.FindFingerPoints(contour, convexHull);
        }
    }
}
