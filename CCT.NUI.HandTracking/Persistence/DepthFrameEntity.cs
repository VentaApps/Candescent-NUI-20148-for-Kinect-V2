using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core;
using System.Xml.Serialization;
using System.IO;

namespace CCT.NUI.HandTracking.Persistence
{
    [Serializable]
    public class DepthFrameEntity
    {
        private ushort[] data;
        private IntSize size;

        public DepthFrameEntity()
        { }
        
        public DepthFrameEntity(IntSize size, ushort[] data)
        {
            this.data = data;
            this.size = size;
        }

        public ushort[] Data
        {
            get { return this.data; }
        }

        [XmlElement(DataType = "base64Binary")]
        public byte[] Binary
        {
            get
            {
                using (var memoryStream = new MemoryStream())
                {
                    new DepthDataFrameRepository(this.Size).Save(this.data, memoryStream);
                    return memoryStream.ToArray();
                }
            }
            set
            {
                using (var memoryStream = new MemoryStream(value))
                {
                    this.data = new DepthDataFrameRepository(this.Size).Load(memoryStream);
                }
            }
        }
  
        public IntSize Size
        {
            get { return this.size; }
            set { this.size = value; }
        }
    }
}
