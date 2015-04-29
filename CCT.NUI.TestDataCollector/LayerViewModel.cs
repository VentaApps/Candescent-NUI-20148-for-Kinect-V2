using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using CCT.NUI.Visual;
using CCT.NUI.Core.Clustering;
using CCT.NUI.HandTracking;

namespace CCT.NUI.TestDataCollector
{
    public class LayerViewModel : INotifyPropertyChanged
    {        
        public bool DisplayClusteringLayer { get; set; }

        public bool DisplayHandLayer { get; set; }

        public void ToggleLayers(IClusterDataSource clusterDataSource, IHandDataSource handDataSource)
        {
            foreach (var layer in this.Layers)
            {
                layer.Dispose();
            }
            var layers = new List<IWpfLayer>();

            if (this.DisplayClusteringLayer && clusterDataSource != null)
            {
                layers.Add(new WpfClusterLayer(clusterDataSource));
            }

            if (this.DisplayHandLayer && handDataSource != null)
            {
                layers.Add(new WpfHandLayer(handDataSource));
            }
            this.Layers = layers;
        }

        private IList<IWpfLayer> layers = new List<IWpfLayer>();

        public IList<IWpfLayer> Layers
        {
            get { return this.layers; }
            private set
            {
                this.layers = value;
                this.OnPropertyChanged("Layers");
            }
        }

        protected void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
