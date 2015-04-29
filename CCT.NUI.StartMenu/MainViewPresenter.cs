using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Drawing;
using System.Reflection;
using CCT.NUI.Core;
using CCT.NUI.Core.OpenNI;
using CCT.NUI.Core.Clustering;
using CCT.NUI.HandTracking;
using CCT.NUI.StartMenu.Persistence;
using CCT.NUI.StartMenu.Properties;
using CCT.NUI.StartMenu.Model;
using CCT.NUI.KinectSDK;

namespace CCT.NUI.StartMenu
{
    internal class MainViewPresenter : Presenter<MainWindow>
    {
        private IDataSourceFactory factory;
        private IHandDataSource handDataSource;

        private IList<IMenuItem> displayedItems;

        private NotifyIconPresenter notifyIconPresenter;

        private IMenuRepository repository = new InMemoryRepository();
        //private IMenuRepository repository = new CsvRepository("menu_config.csv");

        private SettingsViewPresenter settingsPresenter;

        private int minNumberOfFingersToShowMenu = 4;

        private CoordinateMap coordinateMap;

        public MainViewPresenter(MainWindow view)
            : base(view)
        {
            this.factory = this.CreateFactory();
            this.LoadMenuItems();

            this.CreateDataSources();
            this.CreateNotifyIconPresenter();

            this.coordinateMap = new CoordinateMap(this.View.Width / this.handDataSource.Width, this.View.Height / this.handDataSource.Height);

            this.settingsPresenter = new SettingsViewPresenter(this.repository);
            this.settingsPresenter.Apply += new EventHandler(settingsPresenter_Apply);

            if (Settings.Default.ShowDepthWindow)
            {
                ShowDepthWindow();
            }
        }

        private void ShowDepthWindow()
        {
            var form = new System.Windows.Forms.Form();
            var depthImageSource = this.factory.CreateDepthBitmapDataSource();
            depthImageSource.Start();
            form.Controls.Add(new CCT.NUI.Visual.VideoControl(depthImageSource));
            form.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            form.Text = "Depth View";
            form.Size = new System.Drawing.Size(460, 370);
            form.Show();
        }
        
        private IDataSourceFactory CreateFactory()
        {
            if (Settings.Default.Library.ToLower() == "sdk")
            {
                return new SDKDataSourceFactory();
            }
            return new OpenNIDataSourceFactory(@"config.xml");
        }

        public IMenuItem SelectedItem { get; private set; }

        private void CreateDataSources()
        {
            var settings = new HandDataSourceSettings();
            settings.DetectCenterOfPalm = false; //Not used, so disabling this makes the app faster
            this.handDataSource = new HandDataSource(this.factory.CreateShapeDataSource(), settings);
            handDataSource.NewDataAvailable += new NewDataHandler<HandCollection>(handDataSource_NewDataAvailable);
            this.handDataSource.Start();
        }

        private void CreateNotifyIconPresenter()
        {
            this.notifyIconPresenter = new NotifyIconPresenter();
            this.notifyIconPresenter.ShowSettings += new EventHandler(notifyIconPresenter_ShowSettings);
            this.notifyIconPresenter.Close += new EventHandler(notifyIconPresenter_Close);
        }

        protected override void RegisterEvents()
        {
            base.RegisterEvents();
            this.View.Closing += new System.ComponentModel.CancelEventHandler(view_Closing);
        }

        private void ShowSettingsView()
        {
            this.settingsPresenter.Show();
        }

        void settingsPresenter_Apply(object sender, EventArgs e)
        {
            this.LoadMenuItems();
        }

        void view_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.notifyIconPresenter != null)
            {
                this.notifyIconPresenter.Dispose();
            }
            this.handDataSource.Stop();
            this.factory.Dispose();
        }

        private void LoadMenuItems()
        {
            this.displayedItems = new List<IMenuItem>();
            this.View.Clear();
            foreach (var menuItem in this.repository.GetAll().First().Items) //TODO: adjust for multiple menus
            {
                if (menuItem.Exists)
                {
                    this.AddMenuItem(menuItem);
                }
            }
            var exitMenuItem = new ExitMenuItem();
            exitMenuItem.RequestExit += new EventHandler(exitMenuItem_RequestExit);
            this.AddMenuItem(exitMenuItem);
        }

        private void AddMenuItem(IMenuItem menuItem)
        {
            this.displayedItems.Add(menuItem);
            this.View.AddItem(menuItem);
        }

        void handDataSource_NewDataAvailable(HandCollection data)
        {
            if (data.IsEmpty)
            {
                return;
            }
            var handData = data.Hands.First();
            if(!handData.HasContour) 
            {
                this.View.Dispatcher.Invoke(new Action(this.View.Hide));
                return;
            }

            this.View.Dispatcher.BeginInvoke(new Action(() => this.UpdateView(handData)));
        }

        private void UpdateView(HandData handData)
        {
            this.View.SetPolygonPoints(coordinateMap.ConvertPoints(handData.Contour.Points));

            var fingerCount = handData.FingerCount;
            if (fingerCount >= minNumberOfFingersToShowMenu)
            {
                this.View.Show();
            }
            else if (fingerCount == 0)
            {
                this.HideView();
            }

            if (this.View.IsVisible)
            {
                this.HoverMenu(coordinateMap.ConvertPoint(handData.FingerPoints.First().Location));
            }
        }

        private void HideView()
        {
            this.View.Hide();
            if (this.SelectedItem != null)
            {
                this.SelectedItem.Start();
                this.SelectedItem = null;
            }
        }

        private void HoverMenu(System.Windows.Point fingerPoint)
        {
            this.View.SelectAtPoint(fingerPoint);

            if (this.View.SelectedIndex >= 0)
            {
                this.SelectedItem = this.displayedItems[this.View.SelectedIndex];
            }
            else
            {
                this.SelectedItem = null;
            }
        }

        void notifyIconPresenter_Close(object sender, EventArgs e)
        {
            this.View.Close();
        }

        void exitMenuItem_RequestExit(object sender, EventArgs e)
        {
            this.View.Close();
        }

        void notifyIconPresenter_ShowSettings(object sender, EventArgs e)
        {
            this.ShowSettingsView();
        }
    }
}
