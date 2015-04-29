using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core;
using System.ComponentModel;
using CCT.NUI.Visual;
using CCT.NUI.HandTracking.Persistence;
using System.Windows.Media;
using CCT.NUI.Core.Video;
using System.Windows.Media.Imaging;

namespace CCT.NUI.TestDataCollector
{
    public class TestDepthFrame : INotifyPropertyChanged
    {
        private bool isSelected;
        private WriteableBitmap image;

        public TestDepthFrame()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Hands = new BindingList<HandDataViewModel>();
        }

        public TestDepthFrame(DepthDataFrame frame)
            : this()
        {
            this.Frame = frame;
        }

        public TestDepthFrame(string id, DepthDataFrame frame, IEnumerable<HandDataViewModel> hands)
        {
            this.Id = id;
            this.Frame = frame;
            this.Hands = new BindingList<HandDataViewModel>(hands.ToList());
        }

        public string Id { get; set; }

        public DepthDataFrame Frame { get; set; }

        public BindingList<HandDataViewModel> Hands { get; set; }

        public ImageSource Image
        {
            get
            {
                if (this.image == null)
                {
                    this.image = new WriteableBitmap(this.Frame.Width, this.Frame.Height, 96, 96, PixelFormats.Bgr24, null);
                    image.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        var factory = new ArrayToPointerFactory();
                        var pointer = factory.CreatePointer(this.Frame.Data);
                        new DepthImageSourceFactory(this.Frame.MaxDepth).CreateImage(this.image, pointer);
                        factory.Destroy(pointer);
                    }));
                }
                return this.image;
            }
        }

        public bool IsSelected 
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;
                this.OnPropertyChanged("IsSelected");
            }
        }

        public HandTracking.Persistence.TestFrameEntity ToTestDepthFrame()
        {
            return new TestFrameEntity(this.Id, new DepthFrameEntity(this.Frame.Size, this.Frame.Data), this.Hands.Select(h => h.ToHandDefinition()));
        }

        protected void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
