using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.StartMenu.Model;

namespace CCT.NUI.StartMenu.Persistence
{
    public interface IMenuRepository
    {
        IList<Menu> GetAll();

        void Add(Menu menu);

        void Remove(Menu menu);

        void Save();

        void Clear();

        void AddRange(IEnumerable<Menu> menus);
    }
}
