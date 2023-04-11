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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void rivelaPosizione(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Zio pera");
            GeoFinder myGeoFinder = new GeoFinder();
            myGeoFinder.trova();

            textForLatitude.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF2CB9F5");
            textForLongitude.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF2CB9F5");
            dateUpdate.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF2CB9F5");
            textForLatitude.Text = myGeoFinder.getLatitude();
            textForLongitude.Text = myGeoFinder.getLongitude();
            dateUpdate.Text = DateTime.Now.ToString();

            MessageBox.Show(myGeoFinder.magicClient());


            return;
        }


        private void closeWindowPersonalized(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Settings win2 = new Settings();
            win2.Show();
        }
    }
}
