﻿using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Forms;
using ContextMenu = System.Windows.Forms.ContextMenu;
using System.Net.NetworkInformation;

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
            //This is done in order to change the GPS data and update time in this window
            ThreadForGPS.Start(this);
        }

        public void createSysTray()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = new System.Drawing.Icon(Properties.Resources.iconForSysTray, 100, 100);
            _notifyIcon.Visible = true;
            _notifyIcon.DoubleClick += (s, args) => Show();
            _notifyIcon.ShowBalloonTip(2, "Traccar Client for Windows", "Traccar Client is currently running", ToolTipIcon.Info);


            _notifyIcon.Click += (s, e) =>
            {
                // Creazione del menu                
                var menu = new System.Windows.Controls.ContextMenu();
                var showMenuItem = new System.Windows.Controls.MenuItem();
                var exitMenuItem = new System.Windows.Controls.MenuItem();

                showMenuItem.Header = "Show window";
                showMenuItem.Click += (s2, e2) =>
                {
                    // Codice per mostrare la finestra dell'applicazione
                    this.Show();
                    this.WindowState = WindowState.Normal;
                };
                exitMenuItem.Header = "Quit";
                exitMenuItem.Click += (s2, e2) =>
                {
                    // Codice per uscire dall'applicazione
                    ThreadForGPS.killThread();
                    System.Windows.Application.Current.Shutdown();
                };
                menu.Items.Add(showMenuItem);
                menu.Items.Add(exitMenuItem);

                // Mostra il menu vicino alla posizione del cursore
                menu.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
                menu.IsOpen = true;
            };
        }

        public void modifySystray(System.Drawing.Icon icon)
        {
            _notifyIcon.Icon = icon;
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

            ThreadForGPS.Start(this);

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

        private void newBackgroundTask(object sender, RoutedEventArgs e)
        {

            if (ThreadForGPS.doesItExist())
            {
                if (ThreadForGPS.getIsPaused())
                {
                    ThreadForGPS.Resume(this);
                }
                else
                {
                    ThreadForGPS.Pause(this);
                }
            }
            else
            {
                ThreadForGPS.Start(this);
            }
        }

        public void playPauseButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e, string value)
        {
            playPauseButton.Background = (Brush)new BrushConverter().ConvertFromString(value); // set the background color to red
        }
    }
}
