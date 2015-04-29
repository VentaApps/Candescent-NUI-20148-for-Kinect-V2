using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CCT.NUI.Core.Clustering;
using CCT.NUI.HandTracking;
using CCT.NUI.Core.Shape;

namespace CCT.NUI.Samples
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        public SettingsForm(ClusterDataSourceSettings clusterSettings, ShapeDataSourceSettings shapeDataSourceSettings, HandDataSourceSettings handDetectionSettings)
            : this()
        {
            this.propertyGridClustering.SelectedObject = clusterSettings;
            this.propertyGridShape.SelectedObject = shapeDataSourceSettings;
            this.propertyGridHandDetection.SelectedObject = handDetectionSettings;
        }

        private void toolStripButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
