using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.StartMenu.Model;

namespace CCT.NUI.StartMenu.Persistence
{
    public class InMemoryRepository : IMenuRepository
    {
        private IList<Menu> menus = new List<Menu>();

        public InMemoryRepository()
        {
            var menu = new Menu("Default");

            menu.AddItem(new MenuItem("Explorer", @"c:\windows\explorer.exe"));
            menu.AddItem(new MenuItem("FireFox", @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe"));
            menu.AddItem(new MenuItem("ThunderBird", @"C:\Program Files (x86)\Mozilla Thunderbird\thunderbird.exe"));
            menu.AddItem(new MenuItem("Word", @"C:\Program Files\Microsoft Office\Office14\winword.exe"));
            menu.AddItem(new MenuItem("Excel", @"C:\Program Files\Microsoft Office\Office14\excel.exe"));
            menu.AddItem(new MenuItem("Notepad", @"C:\Windows\notepad.exe"));

            this.menus.Add(menu);
        }

        public IList<Menu> GetAll()
        {
            return new List<Menu>(this.menus);
        }

        public void AddRange(IEnumerable<Menu> menus)
        {
            foreach (var item in menus)
            {
                this.Add(item);
            }
        }

        public void Add(Menu item)
        {
            if (!this.Contains(item))
            {
                this.menus.Add(item);
            }
        }

        public void Remove(Menu item)
        {
            if (this.Contains(item))
            {
                this.menus.Remove(item);
            }
        }

        public void Clear()
        {
            this.menus.Clear();
        }

        public void Save()
        { }

        private bool Contains(Menu item)
        {
            return this.menus.Contains(item);
        }
    }
}
