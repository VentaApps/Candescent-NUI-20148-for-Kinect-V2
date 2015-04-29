using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using CCT.NUI.Core;
using CCT.NUI.HandTracking;
using System.Xml.Serialization;

namespace CCT.NUI.HandTracking.Persistence
{
    [XmlType(TypeName = "HandDefinition")]
    public class HandEntity : IHand
    {
        private List<FingerEntity> fingerPoints;

        public HandEntity()
        { }

        public HandEntity(string id)
        {
            this.Id = id;
            this.fingerPoints = new List<FingerEntity>();
        }

        public HandEntity(string id, Point? palmPoint, IEnumerable<FingerEntity> fingers)
        {
            this.Id = id;
            this.PalmPoint = palmPoint;
            this.fingerPoints = fingers.ToList();
        }

        public string Id { get; set; }

        public Point? PalmPoint { get; set; }

        public bool HasPalmPoint
        {
            get { return this.PalmPoint.HasValue; }
        }

        public float PalmX
        {
            get { return this.PalmPoint.GetValueOrDefault().X; }
        }

        public float PalmY
        {
            get { return this.PalmPoint.GetValueOrDefault().Y; }
        }

        public bool HasFingers
        {
            get { return this.FingerCount > 0; }
        }

        public int FingerCount
        {
            get { return this.fingerPoints.Count; }
        }

        public List<FingerEntity> FingerPoints
        {
            get { return this.fingerPoints; }
            set { this.fingerPoints = value; }
        }

        public IEnumerable<IFinger> Fingers
        {
            get { return this.fingerPoints; }
        }

        public Point Location
        {
            get { return this.PalmPoint.Value; }
        }
    }
}
