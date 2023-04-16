using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System;

namespace Client_for_Traccar
{
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            loadLinkServer();
            loadConnectionTime();
            loadDevices();
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

        private void addDeviceButton_click(object sender, RoutedEventArgs e)
        {
            // Apri una finestra di dialogo per chiedere all'utente di inserire un nuovo elemento
            string newItem = addDeviceName_textBox.Text;
            RadioButton newRadioButton = new RadioButton();
            newRadioButton.Style = (Style)Application.Current.FindResource("radioButtonDevice");
            newRadioButton.Content = newItem;
            newRadioButton.GroupName = "devicesRadios";


            if (!string.IsNullOrWhiteSpace(newItem))
            {
                // Create new ItemComboBox
                ComboBoxItem newItemComboBox = new ComboBoxItem();
                newItemComboBox.Style = (Style)Application.Current.FindResource("generalMenuItem"); //applying style
                newItemComboBox.Content = newRadioButton;
                newRadioButton.Click += RadioButton_Checked; //adding click event

                // Adding the previous element to the list
                devicesMenu.Items.Add(newItemComboBox);

                // Save the list into user settings
                StringCollection comboBoxItems = new StringCollection();
                foreach (var item in devicesMenu.Items)
                {
                    if (item is ComboBoxItem comboBoxItem)
                    {
                        RadioButton radioButton = comboBoxItem.Content as RadioButton;
                        if (radioButton != null)
                        {
                            comboBoxItems.Add(radioButton.Content.ToString());
                            Console.WriteLine("Adding radio button: " + radioButton.Content.ToString());
                        }
                    }
                }
                Properties.Settings.Default.devicesListRes = comboBoxItems;
                Properties.Settings.Default.Save();
            }
        }

        public void loadDevices()
        {
            if (Properties.Settings.Default.devicesListRes.ToString() != "defaultValue")
            {
                foreach (string value in Properties.Settings.Default.devicesListRes)
                {
                    string newItem = value;
                    RadioButton newRadioButton = new RadioButton();
                    newRadioButton.Style = (Style)Application.Current.FindResource("radioButtonDevice");
                    newRadioButton.Content = newItem;
                    newRadioButton.GroupName = "devicesRadios";
                    newRadioButton.Click += RadioButton_Checked;

                    ComboBoxItem newItemComboBox = new ComboBoxItem();
                    newItemComboBox.Style = (Style)Application.Current.FindResource("generalMenuItem");
                    newItemComboBox.Content = newRadioButton;

                    if (value == Properties.Settings.Default.defaultDeviceName || Properties.Settings.Default.defaultDeviceName == "defaultWrong")
                    {
                        newRadioButton.IsChecked = true;
                    }

                    devicesMenu.Items.Add(newItemComboBox);
                }
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                // Get the value of selected button
                string selectedValue = radioButton.Content.ToString();

                // Save value in user settings
                Properties.Settings.Default.defaultDeviceName = selectedValue;
                Properties.Settings.Default.Save();
                MessageBox.Show("New value for defaultDeviceName is: " + Properties.Settings.Default.defaultDeviceName);
            }
        }

        private void deleteDevices(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.devicesListRes.Clear();
            for (int i = devicesMenu.Items.Count - 1; i >= 0; i--)
            {
                if (devicesMenu.Items[i] is ComboBoxItem)
                {
                    devicesMenu.Items.RemoveAt(i);
                }
            }
        }
    }
}
