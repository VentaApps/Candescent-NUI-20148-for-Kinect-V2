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
using System.Windows.Shapes;

namespace CCT.NUI.StartMenu
{
    /// <summary>
    /// Interaction logic for ExceptionView.xaml
    /// </summary>
    public partial class ExceptionView : Window
    {
        public ExceptionView()
        {
            InitializeComponent();
        }

        public ExceptionView(Exception exception)
            : this()
        {
            this.textBlock.Text = exception.Message;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
