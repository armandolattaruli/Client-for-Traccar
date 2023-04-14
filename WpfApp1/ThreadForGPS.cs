using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Client_for_Traccar
{
    public static class ThreadForGPS
    {
        private static Thread thread;
        private static bool isPaused;
        private static int intervalInSeconds;

        public static void Start(int interval, MainWindow mainWindow)
        {
            intervalInSeconds = interval;
            thread = new Thread(() =>
            {
                while (true)
                {
                    if (!isPaused)
                    {
                        // qui inserisci il codice che vuoi eseguire ogni intervalInSeconds secondi
                        Client_for_Traccar.GeoFinder.findPosition();
                        GeoFinder.magicClient();

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            mainWindow.textForLatitude.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF2CB9F5");
                            mainWindow.textForLongitude.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF2CB9F5");
                            mainWindow.dateUpdate.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF2CB9F5");
                            mainWindow.textForLatitude.Text = GeoFinder.getLatitude();
                            mainWindow.textForLongitude.Text = GeoFinder.getLongitude();
                            mainWindow.dateUpdate.Text = DateTime.Now.ToString();
                        });
                        Console.WriteLine("Thread eseguito");

                        Thread.Sleep(intervalInSeconds * 1000);
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
            });
            thread.Start();
            MessageBox.Show("Thread startato");
        }

        public static void Pause()
        {
            isPaused = true;
            MessageBox.Show("Service paused");
            Console.WriteLine("Service paused");
        }

        public static void Resume()
        {
            isPaused = false;
            MessageBox.Show("Service restarted");
            Console.WriteLine("Service restarted");
        }

        public static Boolean getIsPaused()
        {
            return isPaused;
        }
    }
}
