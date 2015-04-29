using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using CCT.NUI.Core;
using System.Xml.Serialization;

namespace CCT.NUI.HandTracking.Persistence
{
    [Serializable]
    [XmlType(TypeName = "FingerPoint")]
    public class FingerEntity : IFinger
    {
        private Point point;

        public FingerEntity()
        { }

        public FingerEntity(Point point)
        {
            this.point = point;
        }

        public float X 
        {
            get { return this.Point.X; }
        }
        
        public float Y
        {
            get { return this.Point.Y;  }
        }

        public virtual Point Point
        {
            get { return this.point; }
            set { this.point = value; }
        }

        public override string ToString()
        {
            return this.point.ToString();
        }

        public Point Fingertip
        {
            get { return this.point; }
        }

        public Point Location
        {
            get { return this.point; }
        }
    }
}
