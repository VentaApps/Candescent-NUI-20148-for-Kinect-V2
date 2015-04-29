using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CCT.NUI.Core;
using CCT.NUI.HandTracking.Persistence;

namespace CCT.NUI.TestDataCollector
{
    public class TestDepthFrameRepository
    {
        private string fileDialogFilter = "Test Depth Frames XML (*.xfrm)|*.xfrm|All Files (*.*)|*.*";

        public TestDepthFrame Load(string path)
        {
            var testFrame = new TestFrameRepository().Load(path);
            return new TestDepthFrame(testFrame.Id, new DepthDataFrame(testFrame.Frame.Size, testFrame.Frame.Data), testFrame.Hands.Select(h => new HandDataViewModel(h.Id, h.PalmPoint, h.FingerPoints.Select(f => new FingerPointViewModel(f.Point)))));
        }

        public TestDepthFrame Load()
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.Filter = this.fileDialogFilter;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return this.Load(dialog.FileName);
                }
            }
            return null;
        }

        public void Save(TestDepthFrame frame)
        {
            using (var dialog = new System.Windows.Forms.SaveFileDialog())
            {
                dialog.Filter = this.fileDialogFilter;
                dialog.FileName = frame.Id + ".xfrm";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    new TestFrameRepository().Save(frame.ToTestDepthFrame(), dialog.FileName);
                }
            }
        }

        public IEnumerable<TestDepthFrame> LoadMany()
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.Filter = this.fileDialogFilter;
                dialog.Multiselect = true;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return dialog.FileNames.Select(f => this.Load(f));
                }
            }
            return new List<TestDepthFrame>();
        }
    }
}
