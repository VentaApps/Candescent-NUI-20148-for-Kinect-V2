using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using CCT.NUI.Core;

namespace CCT.NUI.HandTracking.Persistence
{
    [Serializable]
    [XmlType(TypeName = "TestDepthFrame")]
    public class TestFrameEntity
    {
        public TestFrameEntity()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Hands = new List<HandEntity>();
        }

        public TestFrameEntity(DepthFrameEntity frame)
            : this()
        {
            this.Frame = frame;
        }

        public TestFrameEntity(DepthFrameEntity frame, IEnumerable<HandEntity> hands)
            : this()
        {
            this.Frame = frame;
            this.Hands = hands.ToList();
        }

        public TestFrameEntity(string id, DepthFrameEntity frame, IEnumerable<HandEntity> hands)
            : this()
        {
            this.Id = id;
            this.Frame = frame;
            this.Hands = hands.ToList();
        }

        public string Id { get; set; }

        public List<HandEntity> Hands { get; set; }

        public DepthFrameEntity Frame { get; set; }
    }
}
