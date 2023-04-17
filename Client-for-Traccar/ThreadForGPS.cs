using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Client_for_Traccar
{
    public static class ThreadForGPS
    {
        private static Thread sender_thread;
        private static bool isPaused;

        public static void pauseStyleSetter(MainWindow mainWindow)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                mainWindow.textForLatitude.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFF56C2C");
                mainWindow.textForLongitude.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFF56C2C");
                mainWindow.dateUpdate.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFF56C2C");
                mainWindow.textForLatitude.Text = GeoFinder.getLatitude();
                mainWindow.textForLongitude.Text = GeoFinder.getLongitude();

                mainWindow.playPauseButton.Background = Brushes.DarkRed;
                mainWindow.playPauseButton.MouseLeave += (s, ev) => mainWindow.playPauseButton_MouseEnter(s, ev, Brushes.DarkRed.ToString());
                mainWindow.playPauseButton.MouseEnter += (s, ev) => mainWindow.playPauseButton_MouseEnter(s, ev, "#bb0000");
                mainWindow.playPauseButton.ToolTip = "Click to resume the sender";
                mainWindow.modifySystray(Properties.Resources.onlyPause);

                mainWindow.threadIsRunning.Visibility = Visibility.Hidden;
                mainWindow.threadSuspended.Visibility = Visibility.Visible;
            });
        }

        public static void resumeStyleSetter(MainWindow mainWindow)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                //set text colors
                mainWindow.textForLatitude.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF2CB9F5");
                mainWindow.textForLongitude.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF2CB9F5");
                mainWindow.dateUpdate.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF2CB9F5");
                mainWindow.textForLatitude.Text = GeoFinder.getLatitude();
                mainWindow.textForLongitude.Text = GeoFinder.getLongitude();
                mainWindow.dateUpdate.Text = DateTime.Now.ToString();

                mainWindow.playPauseButton.Background = (Brush)new BrushConverter().ConvertFromString("#009900");
                mainWindow.playPauseButton.MouseEnter += (s, ev) => mainWindow.playPauseButton_MouseEnter(s, ev, "#00bb00");
                mainWindow.playPauseButton.MouseLeave += (s, ev) => mainWindow.playPauseButton_MouseEnter(s, ev, "#009900");
                mainWindow.playPauseButton.ToolTip = "Click to pause the sender";
                mainWindow.modifySystray(Properties.Resources.iconForSysTray);

                mainWindow.threadIsRunning.Visibility = Visibility.Visible;
                mainWindow.threadSuspended.Visibility = Visibility.Hidden;
            });
        }

        public static void Start(MainWindow mainWindow)
        {

            if (GeoFinder.variousChecks())
            {
                sender_thread = new Thread(() =>
                {
                    while (true)
                    {
                        if (!isPaused)
                        {
                            Client_for_Traccar.GeoFinder.findPosition();
                            GeoFinder.magicClient(mainWindow);

                            resumeStyleSetter(mainWindow);

                            Thread.Sleep(Properties.Settings.Default.connectionTimeOut * 1000);
                            Console.WriteLine("Thread being executed...");
                        }
                        else
                        {
                            pauseStyleSetter(mainWindow);
                            Thread.Sleep(100);
                        }
                    }
                });
                sender_thread.Start();
                Console.WriteLine("Thread started ");
            }
        }

        public static void Pause(MainWindow mainWindow)
        {
            isPaused = true;
            Console.WriteLine("Service paused");
            pauseStyleSetter(mainWindow);
        }

        public static void Resume(MainWindow mainWindow)
        {
            isPaused = false;
            Console.WriteLine("Service restarted");
            resumeStyleSetter(mainWindow);
        }

        public static Boolean getIsPaused()
        {
            return isPaused;
        }

        public static Boolean doesItExist()
        {
            return sender_thread.IsAlive;
        }

        public static void killThread()
        {
            if (sender_thread.IsAlive)
            {
                sender_thread.Abort();
            }
        }
    }
}
