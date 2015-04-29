using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Effects;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace CCT.NUI.StartMenu
{
    internal class StartMenuViewItemFactory
    {
        private RadialGradientBrush captionBrush;
        public const double DEFAULT_OPACITY = 0.65;
        private int count;
        private Size targetSize = new Size(48, 48);

        public StartMenuViewItemFactory()
        {
            this.CreateCaptionBrush();
        }

        public void Reset()
        {
            this.count = 0;
        }

        public StackPanel Create(Model.IMenuItem item)
        {
            Image image = null;
            using (var icon = item.LoadIcon())
            {
                image = CreateImage(icon);
            }
            var panel = new StackPanel();
            panel.Orientation = Orientation.Vertical;
            panel.Children.Add(image);
            panel.Opacity = DEFAULT_OPACITY;

            var label = CreateLabel(item.Label);
            panel.Children.Add(label);
            panel.Tag = this.count++;
            return panel;
        }

        private void CreateCaptionBrush()
        {
            var color = Color.FromArgb(155, 255, 255, 255);
            this.captionBrush = new RadialGradientBrush(color, Color.FromArgb(0, 255, 255, 255));
            captionBrush.GradientStops.Add(new GradientStop(color, 0.9));
        }

        private Image CreateImage(System.Drawing.Icon icon)
        {
            var image = new Image();
            var imageSrc = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, new Int32Rect(0, 0, icon.Width, icon.Height), BitmapSizeOptions.FromEmptyOptions());
            image.Source = imageSrc;
            image.Width = targetSize.Width;
            image.Height = targetSize.Height;
            image.Margin = new Thickness(10, 0, 10, 5);
            image.Effect = new DropShadowEffect();
            return image;
        }

        private Label CreateLabel(string caption)
        {
            var label = new Label();
            label.Content = caption;
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            label.VerticalAlignment = VerticalAlignment.Top;
            label.Foreground = Brushes.Black;
            label.Background = this.captionBrush;
            return label;
        }
    }
}
