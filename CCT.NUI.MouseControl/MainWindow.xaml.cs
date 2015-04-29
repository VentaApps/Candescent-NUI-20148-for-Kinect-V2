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
using CCT.NUI.Core.OpenNI;
using CCT.NUI.HandTracking;
using CCT.NUI.HandTracking.Mouse;

namespace CCT.NUI.MouseControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OpenNIDataSourceFactory factory;
        private IHandDataSource handDataSource;        
        private TrackingClusterDataSource trackingClusterDataSource;        
        private MouseController mouseController;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonToggle_Click(object sender, RoutedEventArgs e)
        {
            this.mouseController.Enabled = !this.mouseController.Enabled;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Setup();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void Setup()
        {
            foreach (var modeCombination in ModeCombination.ValidCombinations)
            {
                this.comboMode.Items.Add(modeCombination);
            }
            this.factory = new OpenNIDataSourceFactory("mouse_config.xml");
            var depthImageDataSource = this.factory.CreateDepthImageDataSource();
            depthImageDataSource.NewDataAvailable += new Core.NewDataHandler<ImageSource>(MainWindow_NewDataAvailable);
            this.trackingClusterDataSource = this.factory.CreateTrackingClusterDataSource();
            depthImageDataSource.Start();
            CreateController();
        }

        private void CreateController()
        {
            this.handDataSource = new HandDataSource(this.factory.CreateShapeDataSource(this.factory.CreateClusterDataSource(new Core.Clustering.ClusterDataSourceSettings { MaximumDepthThreshold = 900 }), new Core.Shape.ShapeDataSourceSettings()));
            this.mouseController = new MouseController(this.handDataSource, this.buttonToggle.IsChecked.Value);
            this.handDataSource.Start();
        }

        private void CreateControllerInTrackingMode()
        {
            this.handDataSource = new HandDataSource(this.factory.CreateShapeDataSource(this.trackingClusterDataSource));
            this.mouseController = new MouseController(this.handDataSource, this.trackingClusterDataSource);
            this.mouseController.Enabled = this.buttonToggle.IsChecked.Value;
            this.handDataSource.Start();
        }

        private void SetMode()
        {
            if (this.mouseController != null)
            {
                var combination = (this.comboMode.SelectedItem as ModeCombination);

                if (combination.CursorMode == CursorMode.HandTracking)
                {
                    this.Stop();
                    CreateControllerInTrackingMode();
                }
                else
                {
                    if (this.mouseController.CursorMode == CursorMode.HandTracking)
                    {
                        this.Stop();
                        this.CreateController();
                    }
                }
                this.mouseController.SetCursorMode(combination.CursorMode);
                this.mouseController.SetClickMode(combination.ClickMode);
            }
        }

        private void Stop()
        {
            if (this.handDataSource != null)
            {
                this.handDataSource.Stop();
            }
            if (this.mouseController != null)
            {
                this.mouseController.Dispose();
            }
        }

        void MainWindow_NewDataAvailable(ImageSource data)
        {
            this.videoControl.ShowImageSource(data);
        }

        private void checkTopmost_Checked(object sender, RoutedEventArgs e)
        {
            this.Topmost = checkTopmost.IsChecked.GetValueOrDefault();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new Action(() =>
            {
                this.handDataSource.Stop();
                this.factory.Dispose();
            }).BeginInvoke(null, null);
        }

        private void comboMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SetMode();
        }
    }
}
