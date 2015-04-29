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
using System.Windows.Media.Effects;
using CCT.NUI.Visual;
using CCT.NUI.Core;
using CCT.NUI.Core.Clustering;
using CCT.NUI.HandTracking;
using System.Windows.Forms.Integration;
using CCT.NUI.Core.Shape;

namespace CCT.NUI.TestDataCollector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel presenter;

        private ClusterDataSourceSettings clusteringSettings = new ClusterDataSourceSettings();
        private ShapeDataSourceSettings shapeSettings = new ShapeDataSourceSettings();
        private HandDataSourceSettings handSettings = new HandDataSourceSettings();

        public MainWindow()
        {
            InitializeComponent();
            this.displayControlEdit.MouseMove += new System.Windows.Input.MouseEventHandler(displayControlEdit_MouseMove);
            this.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);
        }

        void presenter_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentFrame")
            {
                this.tabControlMain.SelectedIndex = 1;
                if (this.presenter.CurrentFrame == null)
                {
                    this.ClearEditLayers();
                    this.displayControlEdit.ClearImage();
                }
            }
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new Action(() =>
            {
                this.presenter.VideoPresenter.Dispose();
            }).BeginInvoke(null, null);
        }

        void displayControlEdit_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var mouseLocation = this.displayControlEdit.MouseLocation;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializePropertyGrid(this.propertyGridClusteringHost, this.clusteringSettings, MainWindow_PropertyValueChanged);
            InitializePropertyGrid(this.propertyGridShapeHost, this.shapeSettings, MainWindow_PropertyValueChanged);
            InitializePropertyGrid(this.propertyGridHandHost, this.handSettings, MainWindow_PropertyValueChanged);

            this.presenter = new MainViewModel(this.clusteringSettings, this.shapeSettings, this.handSettings);
            this.DataContext = presenter;
            this.displayControl.DataContext = presenter;
            this.presenter.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(presenter_PropertyChanged);
        }

        private void InitializePropertyGrid(WindowsFormsHost host, object selectedObject, System.Windows.Forms.PropertyValueChangedEventHandler eventHandler)
        {
            (host.Child as System.Windows.Forms.PropertyGrid).SelectedObject = selectedObject;
            (this.propertyGridClusteringHost.Child as System.Windows.Forms.PropertyGrid).PropertyValueChanged += eventHandler;
        }

        void bitmapControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.presenter.SelectFrameCommand.Execute((sender as WpfVideoControl).DataContext as TestDepthFrame);
        }

        public void ClearEditLayers()
        {
            foreach (var layer in this.displayControlEdit.Layers)
            {
                layer.Dispose();
            }
        }

        private void handControl_Close(object sender, HandRoutedEventArgs e)
        {
            this.presenter.RemoveHandCommand.Execute(e.HandData);
        }

        private void handControl_MarkPalmCenter(object sender, HandRoutedEventArgs e)
        {
            this.presenter.MarkPalmCenterCommand.Execute(e.HandData);
            SetModeLabel("Center of Palm", e.HandData);
        }

        private void handControl_MarkFingers(object sender, HandRoutedEventArgs e)
        {
            this.presenter.MarkFingersCommand.Execute(e.HandData);
            SetModeLabel("Fingers", e.HandData);
        }

        private void SetModeLabel(string mode, HandDataViewModel handData)
        {
            this.labelMode.Content = string.Format("Mark {0}  of Hand #{1}", mode, handData.Id);
        }

        private void MainWindow_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
        {
            this.presenter.OptionChangeCommand.Execute(null);
        }

        private void displayControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.presenter.SelectPointCommand.Execute(null);
        }

        private void panelHands_RemoveFinger(object sender, FingerRoutedEventArgs e)
        {
            this.presenter.RemoveFingerCommand.Execute(e);
        }

        private void buttonStartDepthSource_Click(object sender, RoutedEventArgs e)
        {
            this.buttonStartDepthSource.IsEnabled = false;
            this.buttonCapture.IsEnabled = true;
            this.buttonCaptureDelayed.IsEnabled = true;
            this.tabControlMain.SelectedIndex = 0;
        }
    }
}
