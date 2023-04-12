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
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            createThread();
        }

        private void createThread()
        {
            // Creare un token di cancellazione
            CancellationTokenSource cts = new CancellationTokenSource();

            // Creare un'istanza del task in background che esegue il lavoro
            Task task = Task.Run(() =>
            {
                while (!cts.IsCancellationRequested)
                {
                    MessageBox.Show("Il task in background sta eseguendo il lavoro...");
                    Thread.Sleep(1000); // Eseguire il lavoro per 1 secondo
                }
            }, cts.Token);

            // Aggiungere un gestore dell'evento per la chiusura dell'applicazione principale
            Application.Current.Exit += (sender, e) =>
            {
                // Annullare il token di cancellazione per terminare il task in background
                cts.Cancel();
            };

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

        private async void pippo()
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
    }
}
