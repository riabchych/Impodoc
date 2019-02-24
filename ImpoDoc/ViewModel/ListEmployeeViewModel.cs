using ImpoDoc.Commands;
using ImpoDoc.Models;
using ImpoDoc.Services;
using ImpoDoc.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ImpoDoc.ViewModel
{
    public class ListEmployeeViewModel : BaseEmployeeViewModel
    {
        public IEmployeeDataService EmployeeService { get; }
        public SingleEmployeeViewModel SingleEmployeeVM { get; }

        public ListEmployeeViewModel()
        {
            EmployeeService = IocKernel.Get<IEmployeeDataService>();
            SingleEmployeeVM = IocKernel.Get<SingleEmployeeViewModel>();
            Task.Run(async () => Employees = new ObservableCollection<Employee>(await EmployeeService.LoadAsync()));
        }

        public ObservableCollection<Employee> Employees
        {
            get { return GetValue(() => Employees); }
            set { SetValue(() => Employees, value); }
        }

        public Employee SelectedEmployee
        {
            get { return GetValue(() => SelectedEmployee); }
            set { SetValue(() => SelectedEmployee, value); }
        }

        private RelayCommand<object> createEmployeeCardCommand;
        public RelayCommand<object> CreateEmployeeCardCommand
        {
            get
            {
                return createEmployeeCardCommand ??
                  (createEmployeeCardCommand = new RelayCommand<object>(obj => ViewEmployeeCard(true)));
            }
        }

        private RelayCommand<object> viewEmployeeCardCommand;
        public RelayCommand<object> ViewEmployeeCardCommand
        {
            get
            {
                return viewEmployeeCardCommand ??
                  (viewEmployeeCardCommand = new RelayCommand<object>(obj => ViewEmployeeCard(), IsEmployeeSelected));
            }
        }

        private RelayCommand<object> removeEmployeeCardCommand;
        public RelayCommand<object> RemoveEmployeeCardCommand
        {
            get
            {
                return removeEmployeeCardCommand ??
                  (removeEmployeeCardCommand = new RelayCommand<object>(obj =>
                  {
                      if (SelectedEmployee == null) return;

                      EmployeeService.Remove(SelectedEmployee);
                      Employees.Remove(SelectedEmployee);
                  }, IsEmployeeSelected));
            }
        }

        private void ViewEmployeeCard(bool isNew = false)
        {

            if (isNew || SelectedEmployee is null)
            {
                isNew = true;
                SingleEmployeeVM.ActiveEmployee = new Employee();
            }
            else
            {
                SingleEmployeeVM.ActiveEmployee = SelectedEmployee.ShallowCopy();
            }

            EmployeeCardWindow employeeCard = IocKernel.Get<EmployeeCardWindow>();

            if (employeeCard.ShowDialog() == true)
            {
                if (isNew)
                {
                    EmployeeService.Add(SingleEmployeeVM.ActiveEmployee);
                    Employees.Add(SingleEmployeeVM.ActiveEmployee);
                }
                else
                {
                    EmployeeService.Update(SingleEmployeeVM.ActiveEmployee);
                    int index = Employees.IndexOf(SelectedEmployee);
                    if (index >= 0 && Employees.Count > index)
                    {
                        Employees[index] = SingleEmployeeVM.ActiveEmployee;
                    }
                }

                employeeCard.Close();
            }
        }

        private bool IsEmployeeSelected(object arg)
        {
            return SelectedEmployee != null;
        }
    }
}
