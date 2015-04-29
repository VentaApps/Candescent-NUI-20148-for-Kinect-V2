using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using CCT.NUI.Visual;
using CCT.NUI.Core;
using CCT.NUI.Core.Video;

namespace CCT.NUI.TestDataCollector
{
    /// <summary>
    /// Interaction logic for DisplayControl.xaml
    /// </summary>
    public partial class DisplayControl : UserControl
    {
        private IImageDataSource imageSource;

        public DisplayControl()
        {
            InitializeComponent();
            this.Layers = new List<IWpfLayer>();
            this.AddEvent(ImageSourceProperty, ImageSourcePropertyChanged);
        }

        private void AddEvent(DependencyProperty property, EventHandler eventHandler)
        {
            DependencyPropertyDescriptor.FromProperty(property, typeof(DisplayControl)).AddValueChanged(this, eventHandler);
        }

        private void ImageSourcePropertyChanged(object sender, EventArgs args)
        {
            if (this.imageSource != null)
            {
                this.imageSource.NewDataAvailable -= new NewDataHandler<System.Windows.Media.ImageSource>(imageSource_NewDataAvailable);
            }
            this.imageSource = this.ImageSource;
            this.imageSource.NewDataAvailable += new NewDataHandler<System.Windows.Media.ImageSource>(imageSource_NewDataAvailable);
        }

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(IImageDataSource), typeof(DisplayControl));

        public IImageDataSource ImageSource
        {
            get { return (IImageDataSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty LayersProperty = DependencyProperty.Register("Layers", typeof(IList<IWpfLayer>), typeof(DisplayControl));

        public IList<IWpfLayer> Layers
        {
            get { return (IList<IWpfLayer>)GetValue(LayersProperty); }
            set { SetValue(LayersProperty, value); }
        }

        public static readonly DependencyProperty MouseLocationProperty = DependencyProperty.Register("MouseLocation", typeof(System.Windows.Point), typeof(DisplayControl));

        public System.Windows.Point MouseLocation
        {
            get { return (System.Windows.Point)GetValue(MouseLocationProperty); }
            set { SetValue(MouseLocationProperty, value); }
        }

        void imageSource_NewDataAvailable(ImageSource data)
        {
            this.videoControl.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.videoControl.ShowImageSource(data);
            }));
        }

        private void videoControl_MouseMove(object sender, MouseEventArgs e)
        {
            this.MouseLocation = e.GetPosition(this.videoControl);
        }

        public void ClearImage()
        {
            this.videoControl.ClearImage();
        }
    }
}
