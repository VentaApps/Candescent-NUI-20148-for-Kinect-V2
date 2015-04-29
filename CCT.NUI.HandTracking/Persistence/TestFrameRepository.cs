using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace CCT.NUI.HandTracking.Persistence
{
    public class TestFrameRepository
    {
        XmlSerializer serializer;

        public TestFrameRepository()
        {
            this.serializer = new XmlSerializer(typeof(TestFrameEntity));
        }

        public TestFrameEntity Load(string path)
        {
            using (var stream = File.Open(path, FileMode.Open))
            {
                return (TestFrameEntity) this.serializer.Deserialize(stream);
            }
        }

        public void Save(TestFrameEntity frame, string path)
        {
            using (var stream = File.Open(path, FileMode.OpenOrCreate))
            {
                this.serializer.Serialize(stream, frame);
            }
        }
    }
}
