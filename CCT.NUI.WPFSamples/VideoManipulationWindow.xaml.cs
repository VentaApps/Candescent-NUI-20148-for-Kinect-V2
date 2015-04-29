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
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using CCT.NUI.Core;
using CCT.NUI.Core.Video;
using CCT.NUI.HandTracking;
using CCT.NUI.WPFSamples.Properties;
using System.IO;

namespace CCT.NUI.WPFSamples
{
    /// <summary>
    /// Interaction logic for VideoManipulationWindow.xaml
    /// </summary>
    public partial class VideoManipulationWindow : Window
    {
        private IHandDataSource handDataSource;
        private IImageDataSource imageSource;

        private bool isNew = true;

        private string[] videoPaths;
        int videoPointer = 0;

        private VideoSurface selectedVideo;
        private IList<VideoSurface> videos;

        private CCT.NUI.Core.Point moveStart;
        private TimeSpan moveStartTime;
        private bool moveMode = false;

        public VideoManipulationWindow()
        {
            InitializeComponent();
            this.videos = new List<VideoSurface>();
            this.videoPaths = Settings.Default.VideoFiles.Split(';');
            CheckVideos();
        }

        private void CheckVideos()
        {
            if (this.videoPaths.Count() == 0 || this.videoPaths.Any(path => !File.Exists(path)))
            {
                MessageBox.Show("There's a problem with your video file paths");
                this.Close();
            }
        }

        public VideoManipulationWindow(IHandDataSource handDataSource, IImageDataSource imageSource)
            : this()
        {
            this.handDataSource = handDataSource;
            this.imageSource = imageSource;
            imageSource.NewDataAvailable += new NewDataHandler<ImageSource>(imageSource_NewDataAvailable);
            handDataSource.NewDataAvailable += new NewDataHandler<HandCollection>(handDataSource_NewDataAvailable);
            imageSource.Start();
        }

        void handDataSource_NewDataAvailable(HandCollection data)
        {
            if (data.Count == 1)
            {
                var hand = data.Hands.First();
                if (hand.FingerCount >= 4)
                {
                    this.StopMode();
                }
                if (hand.FingerCount == 1)
                {
                    this.Select(hand.FingerPoints.First());
                }
            }
            if (data.Count == 2)
            {
                var leftHand = data.Hands.OrderBy(h => h.Location.X).First();
                var rightHand = data.Hands.OrderBy(h => h.Location.X).Last();
                if (leftHand.FingerCount == 2 && rightHand.FingerCount == 2)
                {
                    SurfaceMode(data);
                }
                else if (leftHand.FingerCount >= 4 && rightHand.FingerCount == 0)
                {
                    StopMode();
                }
                else if (leftHand.FingerCount >= 4 && rightHand.FingerCount >= 1)
                {
                    TimeShiftMode(rightHand);
                }
                else if (rightHand.FingerCount == 0 && leftHand.FingerCount == 0)
                {
                    isNew = true;
                    DisabeMoveMode();
                }
                else if (leftHand.FingerCount == 0)
                {
                    DisabeMoveMode();
                }
                else if (rightHand.FingerCount >= 4 && leftHand.FingerCount == 1)
                {
                    CancelMode(leftHand);
                }
            }
            else
            {
                isNew = true;                
            }
        }

        private void Select(FingerPoint fingerTip)
        {
            ExecuteOnHitResult(fingerTip.Location, (hitTestResult) =>
            {
                this.selectedVideo = GetByHitTest(hitTestResult as RayMeshGeometry3DHitTestResult);
                foreach (var video in this.videos.Where(v => v != selectedVideo))
                {
                    video.IsSelected = false;
                }
                this.selectedVideo.IsSelected = true;
            });
        }

        private void StopMode() 
        {
            if (this.selectedVideo != null)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    this.selectedVideo.Pause();
                }));
            }
        }

        private void CancelMode(HandData leftHand)
        {
            var fingerTip = leftHand.Fingers.First().Fingertip;
            ExecuteOnHitResult(fingerTip, (hitTestResult) =>
                {
                    this.Remove(GetByHitTest(hitTestResult));
                });
        }

        private void ExecuteOnHitResult(CCT.NUI.Core.Point fingerTip, Action<RayMeshGeometry3DHitTestResult> action)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                var hitTestResult = VisualTreeHelper.HitTest(this.viewPort, this.canvas.TranslatePoint(new System.Windows.Point(fingerTip.X, fingerTip.Y), this.canvas));
                if (hitTestResult is RayMeshGeometry3DHitTestResult)
                {
                    action(hitTestResult as RayMeshGeometry3DHitTestResult);
                }
            }));
        }

        private VideoSurface GetByHitTest(RayMeshGeometry3DHitTestResult result)
        {
            return this.videos.Where(v => v.ModelVisual3D == result.VisualHit as ModelVisual3D).First();
        }

        private void TimeShiftMode(HandData rightHand)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                if (this.selectedVideo != null && this.selectedVideo.IsPaused)
                {
                    this.selectedVideo.Play();
                }
                var rightFinger = rightHand.Fingers.OrderBy(f => f.Location.X).FirstOrDefault();
                if (rightFinger != null)
                {
                    if (!moveMode)
                    {
                        if (this.selectedVideo !=null && this.selectedVideo.Duration.HasTimeSpan)
                        {
                            moveMode = true;
                            moveStartTime = this.selectedVideo.Position;
                            moveStart = rightFinger.Location;
                            this.slider.Opacity = 0.8;
                            this.slider.Maximum = this.selectedVideo.Duration.TimeSpan.TotalMilliseconds;
                            this.slider.Value = this.selectedVideo.Position.TotalMilliseconds;
                            this.slider.SetValue(Canvas.LeftProperty, (double)rightFinger.Location.X);
                            this.slider.SetValue(Canvas.TopProperty, (double)rightFinger.Location.Y);
                        }
                    }
                    else
                    {
                        this.CalcTimeSpan(rightFinger.Fingertip);
                        this.selectedVideo.Position = TimeSpan.FromMilliseconds(this.slider.Value);
                        this.slider.SetValue(Canvas.TopProperty, (double)rightFinger.Location.Y);
                    }
                }
            }));
        }

        private void SurfaceMode(HandCollection data)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                if (isNew)
                {
                    if (this.selectedVideo != null)
                    {
                        this.selectedVideo.IsSelected = false;
                    }
                    this.selectedVideo = new VideoSurface(this.videoPaths[this.videoPointer++]);
                    this.videos.Add(this.selectedVideo);
                    this.selectedVideo.RequestRemove += new EventHandler(videoSurface_RequestRemove);
                    if (videoPointer >= this.videoPaths.Length)
                    {
                        videoPointer = 0;
                    }

                    this.viewPort.Children.Add(selectedVideo.ModelVisual3D);
                    this.selectedVideo.Play();
                    this.selectedVideo.Opacity = 0.8;
                    isNew = false;
                }
                if (selectedVideo.IsPaused)
                {
                    selectedVideo.Play();
                }

                var points = new List<CCT.NUI.Core.Point>();

                var hand1 = data.Hands[0];
                var hand2 = data.Hands[1];

                points.Add(hand1.FingerPoints[0].Fingertip);
                points.Add(hand1.FingerPoints[1].Fingertip);
                points.Add(hand2.FingerPoints[0].Fingertip);
                points.Add(hand2.FingerPoints[1].Fingertip);

                points = points.OrderBy((p) => p.X).ToList();
                var leftPoints = points.Take(2).ToList();
                var rightPoints = points.Skip(2).Take(2).ToList();
                leftPoints = leftPoints.OrderByDescending(p => p.Y).ToList();
                rightPoints = rightPoints.OrderByDescending(p => p.Y).ToList();

                this.selectedVideo.SetPoints(Map(leftPoints[0]), Map(rightPoints[0]), Map(rightPoints[1]), Map(leftPoints[1]));
            }));
            moveMode = false;
        }

        private void DisabeMoveMode()
        {
            this.Dispatcher.Invoke(new Action(() => 
            {
                this.slider.Opacity = 0;
                moveMode = false;
            }));
        }

        private void CalcTimeSpan(CCT.NUI.Core.Point point)
        {
            var startTime = moveStartTime.TotalMilliseconds;
            var newTime = Math.Abs(startTime + (point.X - moveStart.X) * 100);
            this.slider.Value = newTime;;
        }

        private void videoSurface_RequestRemove(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                var surface =(sender as VideoSurface);
                Remove(surface);
            }));
        }

        private void Remove(VideoSurface videoSurface)
        {
            this.videos.Remove(videoSurface);
            this.viewPort.Children.Remove(videoSurface.ModelVisual3D);
        }

        private Point3D Map(CCT.NUI.Core.Point point)
        {
            return new Point3D(point.X, (this.imageSource.Height - point.Y), 800 - point.Z);
        }

        private void imageSource_NewDataAvailable(ImageSource data)
        {
            this.videoControl.Dispatcher.Invoke(new Action(() => {
                lock (this.imageSource)
                {
                    this.videoControl.ShowImageSource(data);
                }
            }));
        }

        private void viewPort_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var result = VisualTreeHelper.HitTest(this.viewPort, e.GetPosition(this.viewPort));
            if (result is RayMeshGeometry3DHitTestResult)
            {
                this.Remove(GetByHitTest(result as RayMeshGeometry3DHitTestResult));
            }
        }
    }
}
