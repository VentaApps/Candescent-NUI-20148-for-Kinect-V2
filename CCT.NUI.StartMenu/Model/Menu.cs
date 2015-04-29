using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.StartMenu.Model
{
    public class Menu
    {
        private IList<IMenuItem> items;

        public Menu(string name)
        {
            this.Name = name;
            this.items = new List<IMenuItem>();
        }

        public string Name { get; set; }

        public int Count
        {
            get { return this.items.Count; }
        }

        public IEnumerable<IMenuItem> Items
        {
            get { return this.items; }
        }

        public void AddItem(IMenuItem item)
        {
            this.items.Add(item);
        }

        public void MoveDown(int index)
        {
            if (index < 0 || index >= this.Count - 1)
            {
                return;
            }
            var item = this.items[index];
            this.items.Insert(index + 2, item);
            this.items.RemoveAt(index);
        }

        public void MoveUp(int index)
        {
            if (index <= 0)
            {
                return;
            }
            var item = this.items[index];
            this.items.RemoveAt(index);
            this.items.Insert(index - 1, item);
        }

        public void RelocateItem(IMenuItem item, int newIndex)
        {
            var indexOf = this.items.IndexOf(item);
            if (newIndex == indexOf)
            {
                return;
            }
            if (newIndex > indexOf)
            {
                newIndex--;
            }
            this.items.Remove(item);
            this.items.Insert(newIndex, item);
        }

        public void RemoveItem(MenuItem item)
        {
            this.items.Remove(item);
        }

        public IMenuItem GetAt(int index)
        {
            return this.items[index];
        }

        public void RemoveItemAt(int index)
        {
            this.items.RemoveAt(index);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Menu))
            {
                return false;
            }
            return this.Name.Equals((obj as Menu).Name);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public override string ToString()
        {
            return this.Name;
        }

        public Menu Clone()
        {
            var result = new Menu(this.Name);
            foreach(var item in this.items) 
            {
                result.AddItem(item.Clone());
            }
            return result;
        }
    }
}
