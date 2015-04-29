using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CCT.NUI.TestDataCollector
{
    [Serializable]
    public class FingerPointViewModel : INotifyPropertyChanged
    {
        private Core.Point point;
        private bool isSelected;

        public FingerPointViewModel(Core.Point point)
        {
            this.point = point;
        }

        public float X 
        {
            get { return this.Point.X; }
        }
        
        public float Y
        {
            get { return this.Point.Y;  }
        }

        public Core.Point Point
        {
            get { return this.point; }
            set 
            {
                this.point = value;
                this.OnPropertyChanged("X");
                this.OnPropertyChanged("Y");
                this.OnPropertyChanged("Point");
            }
        }

        public bool IsSelected
        {
            get { return this.isSelected; }
            set 
            {
                this.isSelected = value;
                this.OnPropertyChanged("IsSelected");
            }
        }

        public override string ToString()
        {
            return this.Point.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
