using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Forms;
using Client_for_Traccar;


namespace Client_for_Traccar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private NotifyIcon _notifyIcon;
        public MainWindow()
        {
            createSysTray();
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SharedData.createTask();


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
            //Close();
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
            SharedData.cancelTask();
        }
    }
}
