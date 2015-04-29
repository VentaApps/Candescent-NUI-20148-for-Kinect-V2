using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.HandTracking
{
    public class HandDataSourceSettings
    {
        public HandDataSourceSettings()
        {
            SetToDefault(this);
        }

        public float MinimumDistanceBetweenFingerPoints { get; set; }
        public float MinimumDistanceIntersectionPoints { get; set; }
        public float MinimumDistanceFingerPointToIntersectionLine { get; set; }
        public float MaximumDistanceBetweenIntersectionPoints { get; set; }

        public bool DetectFingers { get; set; }
        public bool DetectCenterOfPalm { get; set; }

        public int PalmAccuracySearchRadius { get; set; }
        public float PalmContourReduction { get; set; }

        public bool DetectFingerDirection { get; set; }
        public float FingerBaseIndexOffset { get; set; }
        public float FingerBaseOffsetDistance { get; set; }

        public int FramesForNewFingerPoint { get; set; }
        public int FramesForDiscontinuedFingerPoint { get; set; }

        public static void SetToDefault(HandDataSourceSettings settings)
        {
            settings.MinimumDistanceBetweenFingerPoints = 25;
            settings.MinimumDistanceIntersectionPoints = 30;
            settings.MinimumDistanceFingerPointToIntersectionLine = 22;
            settings.MaximumDistanceBetweenIntersectionPoints = 27;

            settings.DetectFingers = true;
            settings.DetectCenterOfPalm = true;
            settings.DetectFingerDirection = true;

            settings.PalmAccuracySearchRadius = 8;
            settings.PalmContourReduction = 8;

            settings.FingerBaseIndexOffset = 10;
            settings.FingerBaseOffsetDistance = 770;

            settings.FramesForNewFingerPoint = 3;
            settings.FramesForDiscontinuedFingerPoint = 2;
        }
    }
}
