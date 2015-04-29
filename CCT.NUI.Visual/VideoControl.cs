using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CCT.NUI.Visual;
using CCT.NUI.Core;
using CCT.NUI.Core.Video;

namespace CCT.NUI.Visual
{
    public partial class VideoControl : UserControl
    {
        private IBitmapDataSource imageSource;
        private volatile Bitmap bitmap = null;

        private IList<ILayer> layers = new List<ILayer>();

        public VideoControl()
        {
            InitializeComponent();
            this.Paint+=new PaintEventHandler(VideoControl_Paint);
            this.MouseMove += new MouseEventHandler(VideoControl_MouseMove);
        }


        public VideoControl(IBitmapDataSource imageSource)
            : this()
        {
            this.SetImageSource(imageSource);
        }

        public System.Drawing.Point MouseLocation { get; private set; }

        public bool Stretch { get; set; }

        public void SetImageSource(IBitmapDataSource imageSource)
        {
            if (this.imageSource != null) 
            {
                imageSource.NewDataAvailable -= new NewDataHandler<Bitmap>(imageSource_NewImageAvailable);    
            }
            this.imageSource = imageSource;
            imageSource.NewDataAvailable += new NewDataHandler<Bitmap>(imageSource_NewImageAvailable);
        }

        public void Clear()
        {
            this.bitmap = null;
            this.ClearLayers();
            this.Invalidate();
        }

        public void AddLayer(ILayer layer)
        {
            layer.RequestRefresh += new EventHandler(layer_RequestRefresh);
            this.layers.Add(layer);
        }

        public void ClearLayers()
        {
            foreach (var layer in this.layers)
            {
                layer.Dispose();
                layer.RequestRefresh -= new EventHandler(layer_RequestRefresh);
            }
            this.layers.Clear();
        }

        public void SetImage(Bitmap newImage)
        {
            this.bitmap = newImage;
            this.Invalidate();
        }

        public event EventHandler NewMouseLocation;

        private void VideoControl_MouseMove(object sender, MouseEventArgs e)
        {
            this.MouseLocation = e.Location;
            if (this.NewMouseLocation != null)
            {
                this.NewMouseLocation(this, EventArgs.Empty);
            }
        }

        void  VideoControl_Paint(object sender, PaintEventArgs e)
        {
            if (this.bitmap != null)
            {
                System.Drawing.Rectangle targetArea = this.GetTargetArea();
                lock (this.bitmap)
                {
                    e.Graphics.DrawImage(this.bitmap, targetArea);
                }
            }
            foreach (var layer in this.layers)
            {
                layer.Paint(e.Graphics);
            }
        }

        private System.Drawing.Rectangle GetTargetArea()
        {
            if (this.Stretch)
            {
                return this.ClientRectangle;
            }
            else
            { 
                System.Drawing.Rectangle targetArea;
                if (this.imageSource == null)
                {
                    targetArea = new System.Drawing.Rectangle(0, 0, 640, 480);
                }
                else 
                {
                    targetArea = new System.Drawing.Rectangle(0, 0, this.imageSource.Width, this.imageSource.Height);
                }
                if (targetArea.Width > this.Width || targetArea.Height > this.Height)
                {
                    decimal ratio = Math.Min((decimal)this.Width / targetArea.Width, (decimal)this.Height / targetArea.Height);
                    targetArea = new System.Drawing.Rectangle(0, 0, (int)(ratio * targetArea.Width), (int)(ratio * targetArea.Height));
                }
                return targetArea;
            }
        }

        void layer_RequestRefresh(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        void imageSource_NewImageAvailable(Bitmap newImage)
        {
            this.SetImage(newImage);
        }
    }
}
