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
        public static CancellationTokenSource myCancelTokeken = new CancellationTokenSource();
        public static TaskStatus taskStatus = new TaskStatus();
        public static Task _task;

        public static TaskStatus getTaskStatus()
        {
            return _task.Status;
        }

        public static void createTask(MainWindow mainWindow)
        {

            _task = Task.Run(async () =>
            {
                while (!myCancelTokeken.Token.IsCancellationRequested)
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

                    Console.WriteLine("Currently running task: " + Task.CurrentId?.ToString());

                    // Wait x seconds before sending position again
                    // x has been set by the user through the settings window
                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(Properties.Settings.Default.connectionTimeOut), myCancelTokeken.Token);
                    }
                    catch (TaskCanceledException)
                    {
                        Console.WriteLine("Task was cancelled.");
                        break;  // exit the loop
                    }
                }
                MessageBox.Show("CANCELLATION HAS BEEN REQUESTED.");
            }, myCancelTokeken.Token);
        }

        public static void StopTask()
        {
            myCancelTokeken.Cancel();
        }
    }
}
