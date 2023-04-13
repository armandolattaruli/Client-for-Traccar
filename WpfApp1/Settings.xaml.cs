﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Threading;
using System.Reflection;

namespace WpfApp1
{
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            loadLinkServer();
        }

        private void loadLinkServer()
        {
            string path = @"..\..\srcs\serverLink.txt";
            string originalLink = "";
            originalLink = File.ReadAllText(path);

            if (originalLink != "")
            {
                serverAddressName.Text = originalLink;
            }
            else
            {
                serverAddressName.Text = "Server link not saved.";
            }

        }

        private void saveServerAddress_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to update the address of you Traccar server?", "Update File", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                writeToFile(@"..\..\srcs\serverLink.txt", serverAddressName);
            }
        }

        private void writeToFile(string path, TextBox myField)
        {
            //string path = @"..\..\srcs\serverLink.txt";
            if (File.Exists(path))
            {
                MessageBox.Show("File presente!");
                File.WriteAllText(path, myField.Text);
            }
            else
            {
                MessageBox.Show("File NON presente!");
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

        private void killSender(object sender, RoutedEventArgs e)
        {
            SharedData.cancelTask();
        }

        private void saveConnectionTimeOut_click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to update the connection time-out value?", "Update File", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                writeToFile("", connectionTimeOut);                
            }
        }
    }
}
