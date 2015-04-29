using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;
using CCT.NUI.StartMenu.Persistence;
using CCT.NUI.StartMenu.Model;
using CCT.NUI.StartMenu.Properties;

namespace CCT.NUI.StartMenu
{
    internal class SettingsViewPresenter : Presenter<SettingsView>
    {
        private IMenuRepository menuRepository;
        private Menu selectedMenu;
        private IList<Menu> menus;

        public SettingsViewPresenter(IMenuRepository menuRepository)
        {
            this.menuRepository = menuRepository;
            this.View = new SettingsView();
        }

        public void Show()
        {
            this.menus = menuRepository.GetAll().Select(m => m.Clone()).ToList();
            if (this.View == null || !this.View.IsLoaded)
            {
                this.View = new SettingsView();
                this.UpdateView();
            }
            if (!this.View.IsVisible)
            {
                this.View.Show();
            }
            this.View.Activate();
        }

        protected override void RegisterEvents()
        {
            base.RegisterEvents();
            this.View.SelectedIndexChanged += new Action<int>(view_SelectedIndexChanged);
            this.View.Apply += new EventHandler(view_Apply);
            this.View.AddItem += new EventHandler(view_AddItem);
            this.View.RemoveItem += new EventHandler(view_RemoveItem);
            this.View.MoveUp += new EventHandler(view_MoveUp);
            this.View.MoveDown += new EventHandler(view_MoveDown);
            this.View.SelectFile += new EventHandler(view_SelectFile);
        }

        private IMenuItem SelectedItem
        {
            get
            {
                if (this.selectedMenu == null || this.View.SelectedItemIndex < 0)
                {
                    return null;
                }
                return this.selectedMenu.GetAt(this.View.SelectedItemIndex);
            }
        }

        private OpenFileDialog CreateSelectFileDialog(MenuItem selectedItem)
        {
            var dialog = new OpenFileDialog();

            if (selectedItem != null)
            {
                if (selectedItem.Exists)
                {
                    dialog.InitialDirectory = Path.GetDirectoryName(selectedItem.FilePath);
                    dialog.FileName = Path.GetFileName(selectedItem.FilePath);
                }
            }
            return dialog;
        }

        private void view_SelectFile(object sender, EventArgs e)
        {
            if (this.SelectedItem == null || !(this.SelectedItem is MenuItem))
            {
                return;
            }
            var dialog = CreateSelectFileDialog(this.SelectedItem as MenuItem);
            if (dialog.ShowDialog().Value)
            {
                (this.SelectedItem as MenuItem).FilePath = dialog.FileName;
                this.UpdateView();
            }
        }

        private void view_Apply(object sender, EventArgs e)
        {
            this.menuRepository.Clear();
            this.menuRepository.AddRange(this.menus);
            this.menuRepository.Save();

            Settings.Default.ShowDepthWindow = this.View.ShowDepthWindow;
            Settings.Default.Save();

            this.View.Close();
            if (this.Apply != null)
            {
                this.Apply(this, EventArgs.Empty);
            }
        }

        private void view_MoveDown(object sender, EventArgs e)
        {
            this.selectedMenu.MoveDown(this.View.SelectedItemIndex);
            this.UpdateView();
        }

        private void view_MoveUp(object sender, EventArgs e)
        {
            this.selectedMenu.MoveUp(this.View.SelectedItemIndex);
            this.UpdateView();
        }

        private void view_RemoveItem(object sender, EventArgs e)
        {
            if (this.View.SelectedItemIndex >= 0)
            {
                this.selectedMenu.RemoveItemAt(this.View.SelectedItemIndex);
                this.UpdateView();
            }
        }

        private void UpdateView()
        {
            this.View.SetMenus(this.menus);
            this.View.ShowDepthWindow = Settings.Default.ShowDepthWindow;
        }

        private void view_AddItem(object sender, EventArgs e)
        {
            var dialog = CreateSelectFileDialog(null);
            if (dialog.ShowDialog().Value)
            {
                this.selectedMenu.AddItem(new MenuItem(Path.GetFileName(dialog.FileName).Split('.').First(), dialog.FileName));
                this.UpdateView();
            }
        }

        private void view_SelectedIndexChanged(int index)
        {
            if (index >= 0)
            {
                this.selectedMenu = this.menus[index];
                this.View.SetItems(selectedMenu.Items);
            }
        }

        public event EventHandler Apply;
    }
}
