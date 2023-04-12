using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            createTask();
        }

        private void createTask()
        {
            Task.Run(async () =>
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    // Il codice che vuoi eseguire
                    pippo();

                    // Attendi per 5 secondi prima di eseguire nuovamente il codice
                    await Task.Delay(TimeSpan.FromSeconds(3));
                }
            }, _cancellationTokenSource.Token);
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


            return;
        }

        private void pippo()
        {
            MessageBox.Show("ciao");
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
            win2.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win2.Show();
        }

        private void killSender(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
