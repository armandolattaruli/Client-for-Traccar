using System;
using System.Windows;
using System.Windows.Input;

namespace Client_for_Traccar
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class personalMessageBox : Window
    {
        public personalMessageBox()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();

            foreach (Window window in (Application.Current.Windows))
            {
                if (window != this)
                {
                    window.IsEnabled = false;
                    Console.WriteLine("Disabling " + window.Name);
                }
            }
        }

        private void MyWindow_Closed(object sender, EventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window != this)
                {
                    window.IsEnabled = true;
                    Console.WriteLine("Enabling " + window.Name);
                }
            }
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}