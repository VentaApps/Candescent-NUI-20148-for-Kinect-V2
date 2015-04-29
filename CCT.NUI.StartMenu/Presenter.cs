using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CCT.NUI.StartMenu
{
    internal class Presenter<TView>
        where TView : Window, new()
    {
        private TView view;

        public Presenter()
            : this(new TView())
        { }

        public Presenter(TView view)
        {
            this.View = view;
        }

        protected virtual void RegisterEvents()
        { }

        protected TView View
        {
            get { return this.view; }
            set
            {
                this.view = value;
                this.RegisterEvents();
            }
        }
    }
}
