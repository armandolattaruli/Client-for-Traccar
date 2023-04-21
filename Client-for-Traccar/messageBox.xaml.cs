﻿using System;
using System.Windows;

namespace Client_for_Traccar
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class personalMessageBox : Window
    {
        public personalMessageBox()
        {
            InitializeComponent();

            foreach (Window window in (Application.Current.Windows))
            {
                if (window != this && this.IsEnabled)
                {
                    window.IsEnabled = false;
                    Console.WriteLine("closing...");
                }
            }
        }

        private void MyWindow_Closed(object sender, EventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window != this && window.IsEnabled)
                {
                    window.IsEnabled = true;
                }
                Console.WriteLine("opening...");
            }
            Close();
        }
    }
}