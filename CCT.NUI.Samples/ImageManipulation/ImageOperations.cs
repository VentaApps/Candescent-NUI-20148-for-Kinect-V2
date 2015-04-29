using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CCT.NUI.Samples.ImageManipulation
{
    public class ImageOperations
    {
        public static decimal ProportionalScaleRatio(int actualWidth, int actualHeight, int targetWidth, int targetHeight)
        {
            decimal ratioX = (decimal)targetWidth / actualWidth;
            decimal ratioY = (decimal)targetHeight / actualHeight;
            return Math.Min(ratioX, ratioY);
        }

        public static Size ProportionalScale(int actualWidth, int actualHeight, int targetWidth, int targetHeight)
        {
            var ratio = ProportionalScaleRatio(actualWidth, actualHeight, targetWidth, targetHeight);
            return new Size((int)Math.Round(actualWidth * ratio), (int)Math.Round(actualHeight * ratio));
        }

        private static Image Resize(Image originalImage, int newWidth, int newHeight, CompositingQuality compositingQuality, SmoothingMode smoothingMode, InterpolationMode interpolationMode, PixelOffsetMode pixelOffsetmode)
        {
            Image result = new Bitmap(newWidth, newHeight);
            using (var graphic = Graphics.FromImage(result))
            {
                graphic.CompositingQuality = compositingQuality;
                graphic.SmoothingMode = smoothingMode;
                graphic.InterpolationMode = interpolationMode;
                graphic.PixelOffsetMode = pixelOffsetmode;

                Rectangle rectangle = new Rectangle(0, 0, newWidth, newHeight);
                graphic.DrawImage(originalImage, rectangle);
                return result;
            }
        }

        public static Image QualityResize(Image originalImage, int newWidth, int newHeight)
        {
            return Resize(originalImage, newWidth, newHeight, CompositingQuality.HighQuality, SmoothingMode.HighQuality, InterpolationMode.HighQualityBicubic, PixelOffsetMode.HighQuality);
        }

        public static Image FastResize(Image originalImage, int newWidth, int newHeight)
        {
            return Resize(originalImage, newWidth, newHeight, CompositingQuality.HighSpeed, SmoothingMode.HighSpeed, InterpolationMode.Low, PixelOffsetMode.HighSpeed);
        }

        public static Image FastProportionalScale(Image image, int targetWidth, int targetHeight)
        {
            var newSize = ProportionalScale(image.Width, image.Height, targetWidth, targetHeight);
            return FastResize(image, newSize.Width, newSize.Height);
        }

        public static Image QualityProportionalScale(Image image, int targetWidth, int targetHeight)
        {
            var newSize = ProportionalScale(image.Width, image.Height, targetWidth, targetHeight);
            return QualityResize(image, newSize.Width, newSize.Height);
        }

        public static Image Crop(Image image, Rectangle area)
        {
            var newImage = new Bitmap(area.Width, area.Height);
            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, new Rectangle(0, 0, area.Width, area.Height), area, GraphicsUnit.Pixel);
            }
            return newImage;
        }

        public static Rectangle ZoomArea(Point startDragLocation, Point endDragLocation, Size originalSize, Size targetSize, Size border)
        {
            var screenArea = GetRectangle(startDragLocation, endDragLocation);
            screenArea.X -= border.Width;
            screenArea.Y -= border.Height;

            var factor = 1m / ProportionalScaleRatio(originalSize.Width, originalSize.Height, targetSize.Width, targetSize.Height);

            if (screenArea.Width > 0 && screenArea.Height > 0)
            {
                return Multiply(screenArea, factor);
            }
            return Rectangle.Empty;
        }

        public static Rectangle Multiply(Rectangle rectangle, decimal factor)
        {
            return new Rectangle((int) (rectangle.X * factor), (int) (rectangle.Y * factor),  (int) (rectangle.Width * factor),  (int) (rectangle.Height * factor));
        }

        public static Rectangle GetRectangle(Point point1, Point point2)
        {
            int x = Math.Min(point1.X, point2.X);
            int y = Math.Min(point1.Y, point2.Y);
            int w = Math.Max(point1.X, point2.X) - x;
            int h = Math.Max(point1.Y, point2.Y) - y;
            return new Rectangle(x, y, w, h);
        }
    }
}
