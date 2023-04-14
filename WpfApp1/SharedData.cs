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
    public static class SharedData
    {
        public static CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        public static void createTask(MainWindow mainWindow)
        {
            Task.Run(async () =>
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {

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

                    // Wait 10 seconds before sending position again
                    await Task.Delay(TimeSpan.FromSeconds(Properties.Settings.Default.connectionTimeOut));
                }
            }, _cancellationTokenSource.Token);
        }

        public static void cancelTask()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
