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
using System.Runtime.InteropServices;
using System.Diagnostics;
using CCT.NUI.Core;
using CCT.NUI.Core.OpenNI;
using CCT.NUI.Core.Video;
using CCT.NUI.Visual;
using CCT.NUI.HandTracking;
using CCT.NUI.WPFSamples.PinCode;
using CCT.NUI.WPFSamples.Properties;
using CCT.NUI.KinectSDK;
using CCT.NUI.Core.Clustering;

namespace CCT.NUI.WPFSamples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IDataSourceFactory factory;
        private IHandDataSource handDataSource;
        private IClusterDataSource clusterDataSource;
        private IImageDataSource rgbImageDataSource;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Start()
        {
            this.Cursor = Cursors.Wait;
            this.buttonHandInterface.IsEnabled = true;
            this.buttonManipulation.IsEnabled = true;
            this.buttonRectangleVideo.IsEnabled = true;
            this.buttonTouch.IsEnabled = true;
            this.checkClusterLayer.IsEnabled = true;
            this.checkHandLayer.IsEnabled = true;
            this.radioKinectSDK.IsEnabled = false;
            this.radioOpenNI.IsEnabled = false;

            if (this.radioKinectSDK.IsChecked.GetValueOrDefault())
            {
                this.factory = new SDKDataSourceFactory();
            }
            else
            {
                this.factory = new OpenNIDataSourceFactory("config.xml");
            }
            this.clusterDataSource = this.factory.CreateClusterDataSource(new Core.Clustering.ClusterDataSourceSettings { MaximumDepthThreshold = 900 });
            this.handDataSource = new HandDataSource(this.factory.CreateShapeDataSource(this.clusterDataSource, new Core.Shape.ShapeDataSourceSettings()));
            this.rgbImageDataSource = this.factory.CreateRGBImageDataSource();
            this.rgbImageDataSource.Start();

            var depthImageSource = this.factory.CreateDepthImageDataSource();
            depthImageSource.NewDataAvailable += new NewDataHandler<ImageSource>(MainWindow_NewDataAvailable);
            depthImageSource.Start();
            handDataSource.Start();
            this.Cursor = Cursors.Arrow;
        }

        void MainWindow_NewDataAvailable(ImageSource data)
        {
            this.videoControl.Dispatcher.Invoke(new Action(() =>
            {
                this.videoControl.ShowImageSource(data);
            }));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new Action(() =>
            {
                this.handDataSource.Stop();
                this.factory.Dispose();
            }).BeginInvoke(null, null);
        }

        private void buttonTouch_Click(object sender, RoutedEventArgs e)
        {
            new TouchWindow(this.handDataSource).Show();
        }

        private void buttonManipulation_Click(object sender, RoutedEventArgs e)
        {
            new ManipulationWindow(this.handDataSource).Show();
        }

        private void nuiLink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
        }

        private void blogLink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
        }

        private void buttonRectangleVideo_Click(object sender, RoutedEventArgs e)
        {
            new VideoManipulationWindow(this.handDataSource, this.factory.CreateRGBImageDataSource()).Show();
        }

        private void checkHandLayer_Click(object sender, RoutedEventArgs e)
        {
            this.ToggleLayers();
        }

        private void checkClusterLayer_Click(object sender, RoutedEventArgs e)
        {
            this.ToggleLayers();
        }

        private void ToggleLayers()
        {
            var layers = new List<IWpfLayer>();
            if (this.checkHandLayer.IsChecked.GetValueOrDefault())
            {
                layers.Add(new WpfHandLayer(this.handDataSource));
            }
            if (this.checkClusterLayer.IsChecked.GetValueOrDefault())
            {
                layers.Add(new WpfClusterLayer(this.clusterDataSource));
            }
            this.videoControl.Layers = layers;
        }

        private void buttonHandInterface_Click(object sender, RoutedEventArgs e)
        {
            if (this.factory is OpenNIDataSourceFactory)
            {
                (this.factory as OpenNIDataSourceFactory).SetAlternativeViewpointCapability();
            }
            new HandInterfaceWindow(this.handDataSource, this.rgbImageDataSource).Show();
        }

        private void radioOpenNI_Checked(object sender, RoutedEventArgs e)
        {
            this.Start();
        }

        private void radioKinectSDK_Checked(object sender, RoutedEventArgs e)
        {
            this.Start();
        }
    }
}
