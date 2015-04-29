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

namespace CCT.NUI.StartMenu
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : Window
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        public int SelectedItemIndex
        {
            get { return this.dataGrid.SelectedIndex; }
        }

        public void SetMenus(IList<Model.Menu> menus)
        {
            this.listBoxMenus.SelectedIndex = -1;
            this.listBoxMenus.ItemsSource = menus;

            if (menus.Count > 0)
            {
                this.listBoxMenus.SelectedIndex = 0;
            }
        }

        public bool ShowDepthWindow
        {
            get { return this.checkShowDepthWindow.IsChecked.Value; }
            set { this.checkShowDepthWindow.IsChecked = value; }
        }

        public void SetItems(IEnumerable<Model.IMenuItem> menuItems)
        {
            this.dataGrid.CancelEdit(DataGridEditingUnit.Row);
            this.dataGrid.ItemsSource = menuItems;
            this.dataGrid.Items.Refresh();
        }

        private void listBoxMenus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SelectedIndexChanged != null)
            {
                this.SelectedIndexChanged(this.listBoxMenus.SelectedIndex);
            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (this.AddItem != null)
            {
                this.AddItem(this, EventArgs.Empty);
            }
            this.dataGrid.SelectedIndex = this.dataGrid.Items.Count - 1;
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (this.RemoveItem != null)
            {
                this.RemoveItem(this, EventArgs.Empty);
            }
        }

        private void buttonUp_Click(object sender, RoutedEventArgs e)
        {
            if (this.MoveUp != null)
            {
                this.MoveUp(this, EventArgs.Empty);
            }
        }

        private void buttonDown_Click(object sender, RoutedEventArgs e)
        {
            if (this.MoveDown != null)
            {
                this.MoveDown(this, EventArgs.Empty);
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            if (this.Apply != null)
            {
                this.Apply(this, EventArgs.Empty);
            }
        }

        private void buttonSelectFile_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectFile != null)
            {
                this.SelectFile(this, EventArgs.Empty);
            }
        }

        public event Action<int> SelectedIndexChanged;

        public event EventHandler AddItem;

        public event EventHandler Apply;

        public event EventHandler RemoveItem;

        public event EventHandler MoveUp;

        public event EventHandler MoveDown;

        public event EventHandler SelectFile;
    }
}
