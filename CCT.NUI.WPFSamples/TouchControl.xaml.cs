using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CCT.NUI.WPFSamples
{
    /// <summary>
    /// Interaction logic for TouchControl.xaml
    /// </summary>
    public partial class TouchControl : UserControl
    {
        public TouchControl()
        {
            InitializeComponent();
        }

        public TouchControl(int id)
            : this()
        {
            this.Label.Content = id;
        }
    }
}
