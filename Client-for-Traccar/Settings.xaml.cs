using System.Windows;
using System.Windows.Input;

namespace Client_for_Traccar
{
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            loadLinkServer();
        }

        //load the saved server in the input box
        public void loadLinkServer()
        {
            serverAddressName.Text = Properties.Settings.Default.serverLink;
        }

        public void loadConnectionTime()
        {
            connectionTimeOut.Text = Properties.Settings.Default.connectionTimeOut.ToString();
        }

        private void saveServerAddress_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to update the address of you Traccar server?", "Update File", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                Properties.Settings.Default.serverLink = serverAddressName.Text;
                Properties.Settings.Default.Save();
            }
        }

        private void MaincloseWindowPersonalized(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void saveConnectionTimeOut_click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to update the connection time-out value?", "Update File", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                int conversionValue = 0;

                int.TryParse(connectionTimeOut.Text, out conversionValue);
                Properties.Settings.Default.connectionTimeOut = conversionValue;

                Properties.Settings.Default.Save();
            }
        }
    }
}
