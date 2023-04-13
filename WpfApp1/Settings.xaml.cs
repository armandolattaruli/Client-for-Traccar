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
using System.IO;
using System.Threading;
using System.Reflection;
using Client_for_Traccar.Properties;

namespace Client_for_Traccar
{
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            loadLinkServer();
        }

        private void loadLinkServer()
        {
                serverAddressName.Text = Properties.Settings.Default.serverLink;
        }

        private void saveServerAddress_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to update the address of you Traccar server?", "Update File", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                Properties.Settings.Default.serverLink = serverAddressName.Text;
            }
        }

        private void MaincloseWindowPersonalized(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void saveConnectionTimeOut_click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to update the connection time-out value?", "Update File", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                int conversionValue = 0;                

                int.TryParse(connectionTimeOut.Text, out conversionValue);
                Properties.Settings.Default.connectionTimeOut = conversionValue;                
            }
        }
    }
}
