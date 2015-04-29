using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CCT.NUI.Core
{
    public class DepthDataFrameRepository
    {
        private IntSize frameSize;

        public DepthDataFrameRepository(IntSize frameSize)
        {
            this.frameSize = frameSize;
        }

        public DepthDataFrame Load(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                var data = Load(fileStream);
                return new DepthDataFrame(this.frameSize.Width, this.frameSize.Height, data);
            }
        }

        public ushort[] Load(Stream stream)
        {
            var data = new ushort[stream.Length / 2];
            using (var reader = new BinaryReader(stream))
            {
                for (int index = 0; index < data.Length; index++)
                {
                    data[index] = reader.ReadUInt16();
                }
            }
            return data;
        }

        public void Save(DepthDataFrame frame, Stream outputStream)
        {
            this.Save(frame.Data, outputStream);
        }

        public void Save(ushort[] data, Stream outputStream)
        {
            using (var writer = new BinaryWriter(outputStream))
            {
                foreach (var value in data)
                {
                    writer.Write(value);
                }
            }
        }

        public void Save(DepthDataFrame frame, string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                this.Save(frame, fileStream);
            }
        }
    }
}
