using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Input;
using System.ComponentModel;
using CCT.NUI.Core;
using CCT.NUI.Core.Clustering;
using CCT.NUI.Visual;
using CCT.NUI.HandTracking;
using CCT.NUI.TestDataCollector.Commands;
using CCT.NUI.Core.OpenNI;
using CCT.NUI.Core.Shape;
using CCT.NUI.Core.Video;

namespace CCT.NUI.TestDataCollector
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private TestDepthFrame currentFrame = null;

        private ClusterDataSourceSettings clusterDataSourceSettings;
        private ShapeDataSourceSettings shapeDataSourceSettings;
        private HandDataSourceSettings handDataSourceSettings;

        private ISelectMode selectMode;

        private IImageDataSource imageSource;
        private IClusterDataSource clusterDataSource;
        private IShapeDataSource shapeDataSource;
        private IHandDataSource handDataSource;

        public MainViewModel(ClusterDataSourceSettings clusterDataSourceSettings, ShapeDataSourceSettings shapeDataSourceSettings, HandDataSourceSettings handDataSourceSettings)
        {
            this.Frames = new BindingList<TestDepthFrame>();
            this.clusterDataSourceSettings = clusterDataSourceSettings;
            this.shapeDataSourceSettings = shapeDataSourceSettings;
            this.handDataSourceSettings = handDataSourceSettings;

            this.VideoPresenter = new VideoViewModel(clusterDataSourceSettings, shapeDataSourceSettings, handDataSourceSettings);
            this.VideoPresenter.NewFrameCaptured += new Action<DepthDataFrame>(VideoPresenter_NewFrameCaptured);

            this.SaveFrameCommand = new RelayCommand(SaveCurrentFrame);
            this.LoadFrameCommand = new RelayCommand(LoadFrames);
            this.SelectPointCommand = new RelayCommand(SelectPoint);
            this.SelectFrameCommand = new RelayCommand<TestDepthFrame>(SelectFrame);
            this.RemoveFrameCommand = new RelayCommand(RemoveFrame);

            this.AddHandCommand = new RelayCommand(AddHand);
            this.RemoveHandCommand = new RelayCommand<HandDataViewModel>(RemoveHand);

            this.MarkPalmCenterCommand = new RelayCommand<HandDataViewModel>(MarkPalmCenter);
            this.MarkFingersCommand = new RelayCommand<HandDataViewModel>(MarkFingers);
            this.RemoveFingerCommand = new RelayCommand<FingerRoutedEventArgs>(RemoveFinger);

            this.OptionChangeCommand = new RelayCommand(ChangeOptions);

            this.LayerViewModel = new LayerViewModel();
        }

        public VideoViewModel VideoPresenter { get; private set; }

        public LayerViewModel LayerViewModel { get; private set; }

        public ICommand SaveFrameCommand { get; private set; }
        public ICommand LoadFrameCommand { get; private set; }
        public ICommand SelectFrameCommand { get; private set; }
        public ICommand RemoveFrameCommand { get; private set; }

        public ICommand AddHandCommand { get; private set; }
        public ICommand RemoveHandCommand { get; private set; }

        public ICommand SelectPointCommand { get; private set; }

        public ICommand OptionChangeCommand { get; private set; }

        public ICommand MarkPalmCenterCommand { get; private set; }
        public ICommand MarkFingersCommand { get; private set; }
        public ICommand RemoveFingerCommand { get; private set; }

        public bool IsFrameSelected 
        {
            get { return this.currentFrame != null; }
        }

        public IImageDataSource ImageSource {
            get 
            {
                return this.imageSource;
            }
            set 
            {
                this.imageSource = value;
                this.OnPropertyChanged("ImageSource");
            }
        }

        public BindingList<TestDepthFrame> Frames
        {
            get; set;
        }

        public TestDepthFrame CurrentFrame
        {
            get { return this.currentFrame; }
            set
            {
                this.currentFrame = value;
                this.OnPropertyChanged("CurrentFrame");
            }
        }

        public void ClearCurrentFrame()
        {
            this.CurrentFrame = null;
            this.OnPropertyChanged("IsFrameSelected");
        }

        private void RemoveFrame()
        {
            if (this.IsFrameSelected)
            {
                this.Frames.Remove(this.CurrentFrame);
                this.ClearCurrentFrame();
            }
            if (this.Frames.Count > 0)
            {
                this.SelectFrame(this.Frames[0]);
            }
        }

        protected void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        void VideoPresenter_NewFrameCaptured(DepthDataFrame depthDataFrame)
        {
            this.AddNewDepthFrame(depthDataFrame);
        }

        private void AddNewDepthFrame(DepthDataFrame depthDataFrame)
        {
            var testFrame = new TestDepthFrame(depthDataFrame);
            this.AddFrame(testFrame);
        }

        private void LoadFrames()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                foreach (var frame in new TestDepthFrameRepository().LoadMany())
                {
                    this.AddFrame(frame);
                    this.SelectFrame(frame);
                }
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void SaveCurrentFrame()
        {
            new TestDepthFrameRepository().Save(this.CurrentFrame);
        }

        private void AddFrame(TestDepthFrame testFrame) 
        {
            this.Frames.Add(testFrame);

            if (!this.VideoPresenter.IsRunning)
            {
                this.SelectFrame(testFrame);
            }
        }

        private void RemoveFinger(FingerRoutedEventArgs e)
        {
            e.HandData.RemoveFinger(e.Point);
        }

        void MarkFingers(HandDataViewModel handData)
        {
            this.selectMode = new MarkFingersMode(handData);
        }

        private System.Windows.Point mouseLocation;

        public System.Windows.Point MouseLocation
        {
            get 
            {
                return mouseLocation;
            }
            set
            {
                this.mouseLocation = value;
                this.OnPropertyChanged("DepthLocation");
            }
        }

        public Point DepthLocation
        {
            get { return new Point((int)this.MouseLocation.X, (int)this.MouseLocation.Y, this.GetZ()); }
        }

        private float GetZ()
        {
            if (!this.IsFrameSelected || this.CurrentFrame.Frame.Width <= MouseLocation.X || CurrentFrame.Frame.Height <= MouseLocation.Y)
            {
                return 0;
            }
            return this.CurrentFrame.Frame[(int)MouseLocation.X, (int)MouseLocation.Y];
        }

        private void SelectPoint()
        {
            if (this.selectMode != null)
            {
                this.selectMode.SelectPoint(this.DepthLocation);
            }
        }

        private void RemoveHand(HandDataViewModel handData)
        {
            this.CurrentFrame.Hands.Remove(handData);
        }

        private void AddHand()
        {
            if (this.IsFrameSelected) 
            {
                var handData = new HandDataViewModel(this.FindId());
                this.CurrentFrame.Hands.Add(handData);
            }
        }

        private string FindId()
        {
            int index = 1;
            while (true)
            {
                var possibleId = index.ToString();
                if (!this.CurrentFrame.Hands.Any(h => h.Id == possibleId))
                {
                    return possibleId;
                }
                index++;
            }
        }

        void MarkPalmCenter(HandDataViewModel handData)
        {
            this.selectMode = new MarkPalmCenterMode(handData);
        }

        private void ChangeOptions()
        {
            if (this.IsFrameSelected)
            {
                RefreshDepthFrame();
            }
            this.VideoPresenter.ChangeOption();
        }

        private void LoadDepthFrame(string path)
        {
            this.AddNewDepthFrame(new DepthDataFrameRepository(new IntSize(640, 480)).Load(path));
        }

        private void SelectFrame(TestDepthFrame depthFrame)
        {
            foreach (var frame in this.Frames.Where(f => f != depthFrame))
            {
                frame.IsSelected = false;
            }
            depthFrame.IsSelected = true;
            this.CurrentFrame = depthFrame;
            this.OnPropertyChanged("IsFrameSelected");
            RefreshDepthFrame();
        }

        private void RefreshDepthFrame()
        {
            using (var depthFrameSource = new DepthFramePointerDataSource(this.CurrentFrame.Frame))
            {
                this.clusterDataSource = new OpenNIClusterDataSource(depthFrameSource, this.clusterDataSourceSettings);
                this.shapeDataSource = new ClusterShapeDataSource(this.clusterDataSource, this.shapeDataSourceSettings);
                this.handDataSource = new HandDataSource(this.shapeDataSource, this.handDataSourceSettings);
                this.ToggleLayers();
                this.ImageSource = new DepthImageDataSource(depthFrameSource);
                this.ImageSource.Start();
                this.handDataSource.Start();
                depthFrameSource.Push();
            }
        }

        private void ToggleLayers() 
        {
            this.LayerViewModel.ToggleLayers(this.clusterDataSource, this.handDataSource);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
