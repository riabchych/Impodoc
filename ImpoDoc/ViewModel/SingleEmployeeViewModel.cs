using ImpoDoc.Commands;
using ImpoDoc.Models;
using ImpoDoc.Views;

namespace ImpoDoc.ViewModel
{
    public class SingleEmployeeViewModel: BaseEmployeeViewModel
    {
        public Employee ActiveEmployee
        {
            get { return GetValue(() => ActiveEmployee); }
            set { SetValue(() => ActiveEmployee, value); }

        }

        private RelayCommand<object> saveCommand;
        public RelayCommand<object> SaveCommand
        {
            get
            {
                return saveCommand ??
                  (saveCommand = new RelayCommand<object>(obj =>
                  {
                      if (obj is EmployeeCardWindow employeeCardWindow)
                      {
                          employeeCardWindow.DialogResult = true;
                      }
                  }, CanSave));
            }
        }

        private RelayCommand<object> closeCommand;
        public RelayCommand<object> CloseCommand
        {
            get
            {
                return closeCommand ??
                  (closeCommand = new RelayCommand<object>(obj =>
                  {
                      if (obj is EmployeeCardWindow employeeCardWindow)
                      {
                          employeeCardWindow.DialogResult = false;
                      }
                  }));
            }
        }

        public void Clear(object parameter)
        {
            ActiveEmployee = new Employee();
        }

        private bool CanSave(object param)
        {
            return !HasError();
        }
    }
}
