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
using System.Threading;
using System.Windows.Threading;
using OpenNI;
using CCT.NUI.Core;
using CCT.NUI.Core.Video;
using CCT.NUI.Core.OpenNI;
using CCT.NUI.HandTracking;

namespace CCT.NUI.WPFSamples.PinCode
{
    /// <summary>
    /// Interaction logic for HandInterfaceWindow.xaml
    /// </summary>
    public partial class HandInterfaceWindow : Window
    {
        private Label label;
        private Label labelCode;
        private Label labelAccess;
        private Label labelData;
        private TextBlock textBlock;

        private int currentTextIndex = 0;

        private HandInterfaceElement element;

        private string secretText = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore";

        private DispatcherTimer textTimer;

        public HandInterfaceWindow()
        {
            InitializeComponent();
        }

        public HandInterfaceWindow(IHandDataSource handDataSource, IImageDataSource imageDataSource)
            : this()
        {
            var brush = new SolidColorBrush(Color.FromArgb(160, 255, 255, 255));
            this.element = new HandInterfaceElement();
            element.CharEnter += new System.Timers.ElapsedEventHandler(element_Tick);
            element.Reset += new EventHandler(element_Reset);
            this.canvas.Children.Add(element);

            this.label = CreateLabel("Please identify...", brush, 50, 50);
            this.labelCode = CreateLabel("", brush, 50, 80);
            this.labelAccess = CreateLabel("", brush, 50, 110);

            this.textBlock = new TextBlock();
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Text = string.Empty;
            this.labelData = CreateLabel(textBlock, brush, 50, 140);
            this.labelData.MaxWidth = 540;
            this.labelData.MaxHeight = 320;

            handDataSource.NewDataAvailable += new NewDataHandler<HandCollection>(handDataSource_NewDataAvailable);
            imageDataSource.NewDataAvailable += new NewDataHandler<ImageSource>(imageDataSource_NewDataAvailable);
        }

        void imageDataSource_NewDataAvailable(ImageSource data)
        {
            this.UpdateImage(data);
        }

        void handDataSource_NewDataAvailable(HandCollection data)
        {
            this.element.Update(data);
        }

        private Label CreateLabel(object content, Brush brush, int left, int top)
        {
            var label = new Label();
            label.Content = content;
            label.FontSize = 24;
            label.Foreground = brush;
            Canvas.SetLeft(label, left);
            Canvas.SetTop(label, top);
            this.canvas.Children.Add(label);
            return label;
        }

        void element_Reset(object sender, EventArgs e)
        {
            this.code = string.Empty;
            this.labelCode.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.labelCode.Content = this.code;
                this.labelAccess.Content = string.Empty;
            }));
        }

        private string code = string.Empty;

        void element_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            code = code + (sender as HandInterfaceElement).Number.ToString();
            this.labelCode.Dispatcher.Invoke(new Action(() =>
            {
                this.labelCode.Content = this.code;
            }));
            if (this.code.Length == 5)
            {
                if (this.code == "52315")
                {
                    this.element.Stop();
                    this.labelAccess.Dispatcher.Invoke(new Action(() =>
                    {
                        this.labelAccess.Foreground = new SolidColorBrush(Color.FromArgb(160, 0, 255, 0));
                        this.labelAccess.Content = "Access granted!";
                        this.textTimer = new DispatcherTimer();
                        this.textTimer.Interval = TimeSpan.FromMilliseconds(10);
                        this.textTimer.Tick += new EventHandler(textTimer_Tick);
                        this.textTimer.Start();
                    }));
                }
                else
                {
                    this.element.FadeOut();
                    this.labelAccess.Dispatcher.Invoke(new Action(() =>
                    {
                        this.labelAccess.Foreground = new SolidColorBrush(Color.FromArgb(160, 255, 0, 0));
                        this.labelAccess.Content = "Access denied!";
                    }));
                }
            }
        }

        void textTimer_Tick(object sender, EventArgs e)
        {
            this.textBlock.Text += this.secretText[currentTextIndex++];
            if (this.currentTextIndex >= this.secretText.Length - 1)
            {
                this.textTimer.Stop();
            }
        }

        private void UpdateImage(ImageSource data)
        {
            this.videoControl.Dispatcher.Invoke(new Action(() =>
            {
                this.videoControl.ShowImageSource(data);
            }));
        }
    }
}
