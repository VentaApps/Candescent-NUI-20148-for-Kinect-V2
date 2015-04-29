using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Point = CCT.NUI.Core.Point;
using Size = CCT.NUI.Core.Size;
using Rectangle = CCT.NUI.Core.Rectangle;
using System.Drawing;

namespace CCT.NUI.Samples.ImageManipulation
{
    public class InteractiveImage
    {
        private Point originalPosition;
        private Size originalSize;

        private Point position = new Point(100, 100, 0);
        private Size size;
        private Transformation transformation = new Transformation();
        private Bitmap originalImage;
        private Bitmap bitmap;
        private static Pen borderPen = new Pen(Brushes.White, 2);

        public InteractiveImage(Bitmap image)
        {
            this.originalImage = image;
            this.bitmap = image;
            this.size = new Size(image.Width, image.Height);
        }

        public InteractiveImage(Bitmap image, int x, int y)
            : this(image)
        {
            this.position = new Point(x, y, 0);
            this.originalPosition = new Point(x, y, 0);
            this.originalSize = new Size(image.Width / 3, image.Height / 3);
            this.SetSize(this.originalSize);
        }

        public void Translate(float dx, float dy)
        {
            if (dx > 200 || dy > 200)
            {
                return;
            }
            this.position.X += dx;
            this.position.Y += dy;
        }

        public void Resize(Point startDragPoint, Point startDragPoint2, Point center1, Point center2)
        {
            this.transformation.FindTransformation(startDragPoint, startDragPoint2, center1, center2);
            var newSize = new Size(this.size.Width * transformation.Scale, this.size.Height * transformation.Scale);
            if (transformation.Scale > 1.4 || transformation.Scale < 0.6)
            {
                return;
            }

            this.position.X -= (newSize.Width - this.size.Width) / 2;
            this.position.Y -= (newSize.Height - this.size.Height) / 2;
            this.SetSize(newSize);
        }

        private void SetSize(Size size)
        {
            this.size = size;
            this.bitmap = (Bitmap) ImageOperations.FastResize(this.originalImage, (int)size.Width, (int)size.Height);
        }

        public bool Hovered { get; set; }

        public bool Selected { get; set; }

        public void Draw(Graphics g)
        {
            var deltaX = position.X + size.Width / 2;
            var deltaY = position.Y + size.Height / 2;
            g.TranslateTransform(deltaX, deltaY);
            g.RotateTransform(this.transformation.Angle);
            g.TranslateTransform(-deltaX, -deltaY);
            g.DrawImage(this.bitmap, position.X, position.Y, this.size.Width, this.size.Height);
            if (this.Hovered)
            {
                g.DrawRectangle(borderPen, position.X, position.Y, this.size.Width, this.size.Height);
            }
            if (this.Selected)
            {
                g.DrawRectangle(Pens.Orange, position.X, position.Y, this.size.Width, this.size.Height);
            }
            g.ResetTransform();
        }

        public Rectangle Area
        {
            get { return new Rectangle(this.position, this.size); }
        }

        internal void Reset()
        {
            this.Hovered = false;
            this.position = new Point(this.originalPosition);
            this.SetSize(this.originalSize);
            this.transformation.Reset();
        }
    }
}
