using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Forms;
using ContextMenu = System.Windows.Forms.ContextMenu;
using MenuItem = System.Windows.Forms.MenuItem;
using System.Windows.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Threading.Tasks;

namespace Client_for_Traccar
{
    public partial class MainWindow : System.Windows.Window
    {
        public NotifyIcon _notifyIcon;
        public ContextMenu _contextMenu;

        public MainWindow()
        {
            createSysTray();
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            //passing this window as parameter.
            //This is done in order to change the GPS data and update time in this window.
            SharedData.createTask(this);
        }

        public void createSysTray()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = new System.Drawing.Icon("../../assets/icon.ico"); // Path all'icona di notifica
            _notifyIcon.Visible = true;
            _notifyIcon.DoubleClick += (s, args) => Show();
            _notifyIcon.ShowBalloonTip(2, "Traccar Client for Windows", "Traccar Client is currently running", ToolTipIcon.Info);


            _notifyIcon.Click += (s, e) =>
            {
                // Creazione del menu                
                var menu = new System.Windows.Controls.ContextMenu();
                var showMenuItem = new System.Windows.Controls.MenuItem();
                var exitMenuItem = new System.Windows.Controls.MenuItem();

                showMenuItem.Header = "Mostra finestra";
                showMenuItem.Click += (s2, e2) =>
                {
                    // Codice per mostrare la finestra dell'applicazione
                    this.Show();
                    this.WindowState = WindowState.Normal;
                };
                exitMenuItem.Header = "Esci";
                exitMenuItem.Click += (s2, e2) =>
                {
                    // Codice per uscire dall'applicazione
                    System.Windows.Application.Current.Shutdown();
                };
                menu.Items.Add(showMenuItem);
                menu.Items.Add(exitMenuItem);

                // Mostra il menu vicino alla posizione del cursore
                menu.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
                menu.IsOpen = true;
            };
        }


        public void revealPosition(object sender, RoutedEventArgs e)
        {
            GeoFinder.findPosition();

            textForLatitude.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF2CB9F5");
            textForLongitude.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF2CB9F5");
            dateUpdate.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF2CB9F5");
            textForLatitude.Text = GeoFinder.getLatitude();
            textForLongitude.Text = GeoFinder.getLongitude();
            dateUpdate.Text = DateTime.Now.ToString();

            return;
        }

        private void closeWindowPersonalized(object sender, RoutedEventArgs e)
        {
            Hide();
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
            //SharedData.cancelTask();
            //System.Windows.MessageBox.Show(SharedData.getTaskStatus().ToString());
            Console.WriteLine(SharedData.getTaskStatus().ToString());
            SharedData.StopTask();


            Console.WriteLine(SharedData.getTaskStatus().ToString());

        }

        private void newBackgroundTask(object sender, RoutedEventArgs e)
        {
            if (SharedData.getTaskStatus().ToString() == TaskStatus.RanToCompletion.ToString() || SharedData.getTaskStatus().ToString() == TaskStatus.WaitingToRun.ToString())
            {
                SharedData.createTask(this);
                System.Windows.MessageBox.Show("New task created! widht" + Task.CurrentId);
            }
            else
            {
                System.Windows.MessageBox.Show("Task is already running");
            }
        }
    }
}
