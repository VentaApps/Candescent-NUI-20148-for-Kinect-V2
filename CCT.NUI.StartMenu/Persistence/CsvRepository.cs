using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CCT.NUI.StartMenu.Model;

namespace CCT.NUI.StartMenu.Persistence
{
    public class CsvRepository : IMenuRepository
    {
        private const char SEPARATOR = ';';

        private string filePath;
        private IList<Menu> menus;

        public CsvRepository(string filePath)
        {
            this.filePath = filePath;
            this.menus = new List<Menu>();
            this.Load();
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

        public void Add(Menu menu)
        {
            if (!this.menus.Contains(menu))
            {
                this.menus.Add(menu);
            }
        }

        public void Remove(Menu menu)
        {
            if (!this.menus.Contains(menu))
            {
                return;
            }
            this.menus.Remove(menu);
        }

        public void Clear()
        {
            this.menus.Clear();
        }

        public void Save()
        {
            using (var writer = new StreamWriter(this.filePath, false))
            {
                foreach (var menu in this.menus)
                {
                    writer.WriteLine(menu.Name);
                    foreach (var menuItem in menu.Items.OfType<MenuItem>())
                    {
                        writer.WriteLine(menuItem.Label + ";" + menuItem.FilePath);
                    }
                }
            }
        }

        private void Load()
        {
            if (!File.Exists(this.filePath))
            {
                return;
            }
            this.menus.Clear();
            using (var reader = new StreamReader(this.filePath))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    this.ReadLine(line);
                }
            }
        }

         private void ReadLine(string line)
         {
             if (!line.Contains(SEPARATOR))
             {
                 this.menus.Add(new Menu(line));
             }
             else
             {
                 var values = line.Split(SEPARATOR);
                 if (values.Length < 2)
                 {
                     throw new IOException("Can't parse line: " + line);
                 }
                 this.menus.Last().AddItem(new MenuItem(values[0], values[1]));
             }
         }
    }
}
