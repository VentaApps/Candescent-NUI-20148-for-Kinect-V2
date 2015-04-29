using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace CCT.NUI.StartMenu.Model
{
    public class MenuItem : IMenuItem
    {
        private string label;
        private string filePath;

        public MenuItem(string text, string filePath)
        {
            this.filePath = filePath;
            this.label = text;
        }

        public string Label
        {
            get { return this.label; }
            set { this.label = value; }
        }

        public bool Exists
        {
            get { return File.Exists(this.filePath); }
        }

        public string FilePath
        {
            get { return this.filePath; }
            set { this.filePath = value; }
        }

        public Icon LoadIcon()
        {
            return Icon.ExtractAssociatedIcon(this.filePath);
        }

        public virtual void Start() 
        {
            if (this.Exists)
            {
                Process.Start(this.filePath);
            }
        }

        public override bool Equals(object obj)
        {
            if(obj == null || !(obj is MenuItem)) 
            {
                return false;
            }
            return this.filePath.Equals(((MenuItem)obj).FilePath);
        }

        public override int GetHashCode()
        {
            return this.filePath.GetHashCode();
        }

        public override string ToString()
        {
            return this.Label;
        }

        public IMenuItem Clone()
        {
            return new MenuItem(this.label, this.filePath);
        }
    }
}
