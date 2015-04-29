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
using System.ComponentModel;

namespace CCT.NUI.Visual
{
    /// <summary>
    /// Interaction logic for WpfVideoControl.xaml
    /// </summary>
    public partial class WpfVideoControl : UserControl
    {
        private IList<IWpfLayer> layers;

        public WpfVideoControl()
        {
            InitializeComponent();
            this.AddEvent(WpfVideoControl.ImageProperty, this.ImagePropertyChanged);
            this.AddEvent(WpfVideoControl.LayersProperty, this.LayersPropertyChanged);
        }
       
        private void AddEvent(DependencyProperty property, EventHandler eventHandler)
        {
            DependencyPropertyDescriptor.FromProperty(property, typeof(WpfVideoControl)).AddValueChanged(this, eventHandler);
        }

        public static readonly DependencyProperty LayersProperty = DependencyProperty.Register("Layers", typeof(IList<IWpfLayer>), typeof(WpfVideoControl));

        public IList<IWpfLayer> Layers
        {
            get { return (IList<IWpfLayer>)GetValue(LayersProperty); }
            set { SetValue(LayersProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(WpfVideoControl));

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(WpfVideoControl));

        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public void ShowImageSource(ImageSource source)
        {
            this.videoImage.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.videoImage.Source = source;
            }));
        }

        private void LayersPropertyChanged(object sender, EventArgs args)
        {
            if (this.layers != null)
            {
                foreach (var layer in this.layers)
                {
                    layer.Dispose();
                }
            }
            if (this.Layers != null)
            {
                foreach (var layer in this.Layers)
                {
                    layer.Activate(this.canvas);
                }
            }
            this.layers = this.Layers;
        }

        private void ImagePropertyChanged(object sender, EventArgs args)
        {
            this.ShowImageSource(this.Image);
        }

        public void ClearImage()
        {
            this.videoImage.Source = null;
        }
    }
}
