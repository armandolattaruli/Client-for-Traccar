using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client_for_Traccar
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            foreach (Window window in Application.Current.Windows)
            {
                if (window != this)
                {
                    window.IsEnabled = false;
                }
            }
        }

        private void MyWindow_Closed(object sender, EventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window != this)
                {
                    window.IsEnabled = true;
                }
            }
        }

    }
}
