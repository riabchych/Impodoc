using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImpoDoc.Views
{
    /// <summary>
    /// Логика взаимодействия для EmployeeListControl.xaml
    /// </summary>
    public partial class CompanyListControl : UserControl
    {
        private static CompanyListControl _instance;
        public static CompanyListControl Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new CompanyListControl();
                }

                return _instance;
            }
        }

        public CompanyListControl()
        {
            InitializeComponent();
        }
    }
}
