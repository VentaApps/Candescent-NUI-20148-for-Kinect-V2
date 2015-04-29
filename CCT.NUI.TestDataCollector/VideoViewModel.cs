using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;
using CCT.NUI.Core.Clustering;
using CCT.NUI.HandTracking;
using CCT.NUI.Core;
using CCT.NUI.Visual;
using CCT.NUI.Core.OpenNI;
using CCT.NUI.Core.Shape;
using CCT.NUI.Core.Video;
using CCT.NUI.TestDataCollector.Commands;

namespace CCT.NUI.TestDataCollector
{
    public class VideoViewModel : INotifyPropertyChanged, IDisposable
    {
        private IClusterDataSource clusterdataSource;
        private IShapeDataSource shapeDataSourceVideo;
        private IHandDataSource handDataSourceVideo;

        private DepthDataFrameFactory depthFrameFactory;

        private IImageDataSource videoSource;

        private ClusterDataSourceSettings clusterDataSourceSettings;
        private ShapeDataSourceSettings shapeDataSourceSettings;
        private HandDataSourceSettings handDataSourceSettings;        
        private OpenNIDataSourceFactory dataSourceFactory;

        public VideoViewModel(ClusterDataSourceSettings clusterDataSourceSettings, ShapeDataSourceSettings shapeDataSourceSettings, HandDataSourceSettings handDataSourceSettings)
        {
            this.clusterDataSourceSettings = clusterDataSourceSettings;
            this.shapeDataSourceSettings = shapeDataSourceSettings;
            this.handDataSourceSettings = handDataSourceSettings;

            this.CaptureFrameCommand = new RelayCommand(CaptureFrame);
            this.CaptureFrameDelayedCommand = new RelayCommand(CaptureFrameDelayed);

            this.StartDepthSourceCommand = new RelayCommand(StartDepthSource);

            this.LayerViewModel = new LayerViewModel();
        }

        public LayerViewModel LayerViewModel { get; set; }

        public ICommand CaptureFrameCommand { get; private set; }
        public ICommand CaptureFrameDelayedCommand { get; private set; }

        public ICommand StartDepthSourceCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }

        public IImageDataSource VideoSource
        {
            get { return this.videoSource; }
            set
            {
                this.videoSource = value;
                this.OnPropertyChanged("VideoSource");
            }
        }

        public event Action<DepthDataFrame> NewFrameCaptured;

        public void ChangeOption()
        {
            if (this.IsRunning)
            {
                this.ToggleLayers();
            }
        }

        public bool IsRunning
        {
            get { return this.VideoSource != null && this.VideoSource.IsRunning; }
        }

        private void CaptureFrameDelayed()
        {
            new Action(() => CaptureFrameDelayed(TimeSpan.FromSeconds(3))).BeginInvoke(null, null);
        }

        private void CaptureFrameDelayed(TimeSpan delay)
        {
            Thread.Sleep(delay);
            Application.Current.Dispatcher.Invoke(new Action(() => this.CaptureFrame()));
        }

        private void CaptureFrame()
        {
            var frame = this.depthFrameFactory.Create(dataSourceFactory.GetDepthGenerator().DataPtr);

            if (this.NewFrameCaptured != null)
            {
                this.NewFrameCaptured(frame);
            }
        }

        public void Dispose()
        {
            if (this.VideoSource != null)
            {
                this.VideoSource.Stop();
            }
            if (this.dataSourceFactory != null)
            {
                this.dataSourceFactory.Dispose();
            }
        }

        private void StartDepthSource()
        {
            try
            {
                if (this.VideoSource == null)
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    this.dataSourceFactory = new OpenNIDataSourceFactory("config.xml");
                    this.VideoSource = dataSourceFactory.CreateDepthImageDataSource();
                    this.clusterdataSource = dataSourceFactory.CreateClusterDataSource(this.clusterDataSourceSettings);
                    this.shapeDataSourceVideo = dataSourceFactory.CreateShapeDataSource(this.clusterdataSource, this.shapeDataSourceSettings);
                    this.handDataSourceVideo = new HandDataSource(this.shapeDataSourceVideo, this.handDataSourceSettings);
                    this.depthFrameFactory = new DepthDataFrameFactory(this.clusterdataSource.Size);
                    this.handDataSourceVideo.Start();
                    this.VideoSource.Start();
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
                this.ToggleLayers();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error: " + exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ToggleLayers()
        {
            this.LayerViewModel.ToggleLayers(this.clusterdataSource, this.handDataSourceVideo);
        }

        protected void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
