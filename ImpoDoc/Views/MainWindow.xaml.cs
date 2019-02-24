using ImpoDoc.Services;
using System.Windows;

namespace ImpoDoc.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            IocKernel.Get<EmployeeListWindow>().Show();
        }
    }
}
