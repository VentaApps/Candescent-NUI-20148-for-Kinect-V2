using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace CCT.NUI.WPFSamples
{
    public class VideoSurface
    {
        private bool isSelected = true;

        public VideoSurface(string mediaSource)
        {
            this.ModelVisual3D = new ModelVisual3D();

            var geometryModel = new GeometryModel3D();
            this.ModelVisual3D.Content = geometryModel;

            this.geometry = new MeshGeometry3D();
            geometryModel.Geometry = geometry;

            var positions = new Point3DCollection();
            positions.Add(new Point3D(0, 0, 0));
            positions.Add(new Point3D(640, 0, 0));
            positions.Add(new Point3D(640, 480, 0));
            positions.Add(new Point3D(0, 480, 0));
            this.geometry.Positions = positions;

            var textureCoordinates = new PointCollection();
            textureCoordinates.Add(new System.Windows.Point(0, 1));
            textureCoordinates.Add(new System.Windows.Point(1, 1));
            textureCoordinates.Add(new System.Windows.Point(1, 0));
            textureCoordinates.Add(new System.Windows.Point(0, 0));
            this.geometry.TextureCoordinates = textureCoordinates;

            var triangleIndices = new Int32Collection();
            triangleIndices.Add(0);
            triangleIndices.Add(1);
            triangleIndices.Add(2);
            triangleIndices.Add(2);
            triangleIndices.Add(3);
            triangleIndices.Add(0);
            this.geometry.TriangleIndices = triangleIndices;

            var material = new EmissiveMaterial();
            var brush = new VisualBrush();
            this.border = new Border();
            this.border.BorderBrush = Brushes.White;
            this.border.BorderThickness = new Thickness(10);
            this.border.Opacity = 0;

            this.mediaElement = new MediaElement();
            mediaElement.LoadedBehavior = MediaState.Manual;
            mediaElement.Source = new Uri(mediaSource);

            this.border.Child = mediaElement;
            brush.Visual = border;
            material.Brush = brush;
            geometryModel.Material = material;

            this.mediaElement.MediaEnded += new RoutedEventHandler(mediaElement_MediaEnded);
        }

        public ModelVisual3D ModelVisual3D
        {
            get;
            private set;
        }

        private MeshGeometry3D geometry;
        private MediaElement mediaElement;
        private Border border;

        public bool IsSelected
        {
            get { return this.isSelected; }
            set 
            {
                this.isSelected = value;
                if (value)
                {
                    this.border.BorderBrush = Brushes.White;
                }
                else
                {
                    this.border.BorderBrush = Brushes.Black;
                }
            }
        }

        public void Play()
        {
            this.mediaElement.Play();
            this.IsPaused = false;
        }

        public void Pause() 
        {
            this.mediaElement.Pause();
            this.IsPaused = true;
        }

        public void Stop()
        {
            this.mediaElement.Stop();
        }

        public bool IsPaused
        {
            get;
            private set;
        }

        public TimeSpan Position
        {
            get { return this.mediaElement.Position; }
            set { this.mediaElement.Position = value; }
        }

        public Duration Duration
        {
            get { return this.mediaElement.NaturalDuration; }
        }

        public double Opacity
        {
            get { return this.border.Opacity; }
            set { this.border.Opacity = value; } 
        }

        public void SetPoints(Point3D p1, Point3D p2, Point3D p3, Point3D p4)
        {
            geometry.Positions[0] = p1;
            geometry.Positions[1] = p2;
            geometry.Positions[2] = p3;
            geometry.Positions[3] = p4;
        }

        void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            this.OnRequestRemove();
        }

        protected void OnRequestRemove()
        {
            if (this.RequestRemove != null)
            {
                this.RequestRemove(this, EventArgs.Empty);
            }
        }
        public event EventHandler RequestRemove;

    }
}
