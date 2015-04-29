using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Windows.Media.Effects;
using CCT.NUI.StartMenu.Persistence;
using System.Windows.Controls;

namespace CCT.NUI.StartMenu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StartMenuViewItemFactory factory = new StartMenuViewItemFactory();
        private IList<StackPanel> panels = new List<StackPanel>();

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                this.SelectedIndex = -1;
                new MainViewPresenter(this);
            }
            catch (Exception exception)
            {
                new ExceptionView(exception).ShowDialog();
                this.Close();
            }
        }

        public void SetPolygonPoints(PointCollection pointCollection)
        {
            this.polygon.Points = pointCollection;
        }

        public int SelectedIndex { get; set; }

        public void SelectAtPoint(Point point)
        {
            var localPanel = this.FindSelectedPanel(point);
            if (localPanel != null)
            {
                this.SelectedIndex = (int)localPanel.Tag;
                localPanel.Opacity = 1;
                this.SetOpacity(this.panels.Except(new List<UIElement> { localPanel }), StartMenuViewItemFactory.DEFAULT_OPACITY);
            }
            else
            {
                this.SetOpacity(this.panels, StartMenuViewItemFactory.DEFAULT_OPACITY);
                this.SelectedIndex = -1;
            }
        }

        private void SetOpacity(IEnumerable<UIElement> elements, double opacity)
        {
            foreach (var element in elements)
            {
                element.Opacity = opacity;
            }
        }

        public void Clear()
        {
            this.factory.Reset();
            this.panels.Clear();
            this.stackPanel.Children.Clear();
        }

        private void DisplayStartMenuItem(Model.IMenuItem menuItem)
        {
            var panel = this.factory.Create(menuItem);
            this.stackPanel.Children.Add(panel);
            this.panels.Add(panel);
        }

        public void AddItem(Model.IMenuItem menuItem)
        {
            this.Dispatcher.Invoke(new Action(() => this.DisplayStartMenuItem(menuItem)));
        }

        private StackPanel FindSelectedPanel(System.Windows.Point point)
        {
            return this.panels.Where((p) =>
            {
                var renderedLocation = p.TranslatePoint(new System.Windows.Point(0, 0), this);
                return point.X >= renderedLocation.X && point.X < renderedLocation.X + p.ActualWidth && point.Y >= renderedLocation.Y && point.Y < renderedLocation.Y + p.ActualHeight;

            }).FirstOrDefault();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}
