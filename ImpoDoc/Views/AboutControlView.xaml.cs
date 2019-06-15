﻿using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace ImpoDoc.Views
{
    /// <summary>
    /// Interaction logic for AboutControlView.xaml
    /// </summary>
    public partial class AboutControlView : UserControl
    {
        public AboutControlView()
        {
            InitializeComponent();
        }

        private void Link_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs eventArgs)
        {
            if (eventArgs.ChangedButton == MouseButton.Left)
            {
                if (Parent is Window parent)
                {
                    parent.DragMove();
                }
            }
        }
    }
}
