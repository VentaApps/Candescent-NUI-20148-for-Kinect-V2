using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using CCT.NUI.HandTracking;
using CCT.NUI.Core;

namespace CCT.NUI.TestDataCollector
{
    [Serializable]
    public class HandDataViewModel : INotifyPropertyChanged
    {
        private BindingList<FingerPointViewModel> fingerPoints;
        private bool isSelected;

        public HandDataViewModel(string id)
        {
            this.Id = id;
            this.fingerPoints = new BindingList<FingerPointViewModel>();
        }

        public HandDataViewModel(string id, Point? palmPoint, IEnumerable<FingerPointViewModel> fingerPoints)
        {
            this.Id = id;
            this.PalmPoint = palmPoint;
            this.fingerPoints = new BindingList<FingerPointViewModel>(fingerPoints.ToList());
        }

        public HandTracking.Persistence.HandEntity ToHandDefinition()
        {
            return new HandTracking.Persistence.HandEntity(this.Id, this.PalmPoint, this.FingerPoints.Select(f => new HandTracking.Persistence.FingerEntity(f.Point)));
        }

        public bool IsSelected 
        {
            get { return this.isSelected; }
            set 
            {
                this.isSelected = value;
                this.PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
            }
        }

        public string Id { get; private set; }

        public Point? PalmPoint { get; private set; }

        public bool HasPalmPoint
        {
            get { return this.PalmPoint.HasValue; }
        }

        public int PalmLeft
        {
            get { return (int)this.PalmPoint.GetValueOrDefault().X; }
        }

        public int PalmTop
        {
            get { return (int)this.PalmPoint.GetValueOrDefault().Y; }
        }

        public string PalmPointText
        {
            get
            {
                if (PalmPoint.HasValue)
                {
                    return PalmPoint.ToString();
                }
                return "not set";
            }
        }

        public bool HasFingers 
        { 
            get { return this.FingerCount > 0; }
        }

        public int FingerCount
        {
            get { return this.fingerPoints.Count; }
        }

        public BindingList<FingerPointViewModel> FingerPoints
        {
            get { return this.fingerPoints; }
        }

        public void MarkFinger(FingerPointViewModel point)
        {
            this.fingerPoints.Add(point);
            NotifyFingerChange();
        }

        public void RemoveFinger(FingerPointViewModel point)
        {
            this.fingerPoints.Remove(point);
            NotifyFingerChange();
        }

        public void MarkCenterOfPalm(Point point)
        {
            this.PalmPoint = point;
            this.OnPropertyChanged("HasPalmPoint");
            this.OnPropertyChanged("PalmLeft");
            this.OnPropertyChanged("PalmTop");
            this.OnPropertyChanged("PalmPoint");
            this.OnPropertyChanged("PalmPointText");
        }

        protected void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyFingerChange()
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs("HasFingers"));
            this.PropertyChanged(this, new PropertyChangedEventArgs("FingerCount"));
        }
    }
}
