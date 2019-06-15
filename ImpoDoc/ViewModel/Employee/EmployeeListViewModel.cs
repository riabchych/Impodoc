using ImpoDoc.Commands;
using ImpoDoc.Common;
using ImpoDoc.Data;
using ImpoDoc.Entities;
using ImpoDoc.Ioc;
using ImpoDoc.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImpoDoc.ViewModel
{
    public class EmployeeListViewModel : ItemListViewModel<Employee>
    {
        protected override ItemDetailsViewModel<Employee> ItemDetailsVM => IocKernel.Get<EmployeeDetailsViewModel>();
        private EmployeeDetailsWindow ItemDetailsWnd => IocKernel.Get<EmployeeDetailsWindow>();
        public override Dictionary<string, string> FilterList => new Dictionary<string, string>
        {
            { "FirstName", Properties.Resources.EmployeeFirstName },
            { "LastName", Properties.Resources.EmployeeLastName},
            { "MiddleName", Properties.Resources.EmployeeMiddleName },
            { "Email", Properties.Resources.EmployeeEmail },
            { "Department", Properties.Resources.EmployeeDepartment },
            { "PhoneNumber", Properties.Resources.EmployeePhoneNumber }
        };

        public async Task LoadDataAsync()
        {
            BusyStatus.Content = Properties.Resources.LoadingEmployeesList;
            using (var context = new DatabaseContext())
            {
                Logger.Debug(Properties.Resources.LoadingEmployeesList);
                List<Employee> items = await Task.Run(() => context.Employees.ToListAsync());
                Logger.Debug(Properties.Resources.LoadedEmployeesList);
                UpdateItemsViewSource(items);
            }
        }

        public override RelayCommand<object> RemoveItemCommand
        {
            get
            {
                return removeItemCommand ??
                  (removeItemCommand = new RelayCommand<object>(obj =>
                  {
                      if (SelectedItem == null)
                      {
                          return;
                      }

                      using (var context = new DatabaseContext())
                      {
                          using (var transaction = context.Database.BeginTransaction())
                          {
                              try
                              {
                                  context.Employees.Remove(SelectedItem);
                                  context.SaveChanges();
                                  _ = Items.Remove(SelectedItem);
                                  transaction.Commit();
                                  Logger.Debug(Properties.Resources.LoggerTransactionRemoveEmployeeExecuted);
                              }
                              catch (Exception e)
                              {
                                  Logger.Debug(Properties.Resources.LoggerTransactionRemoveEmployeeError);
                                  Logger.Error(e.StackTrace);
                                  transaction.Rollback();
                              }
                          }

                      }
                  }, IsItemSelected));
            }
        }

        protected override void ViewItemDetailsAsync(bool isNew = false)
        {
            ItemDetailsVM.ActiveItem = isNew || SelectedItem is null ? new Employee() : Utils.CloneObject(SelectedItem);

            if (ItemDetailsWnd.ShowDialog() == true)
            {
                using (var context = new DatabaseContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (isNew)
                            {
                                context.Employees.Add(ItemDetailsVM.ActiveItem);
                                context.SaveChanges();
                                Items.Add(ItemDetailsVM.ActiveItem);
                                Logger.Debug(Properties.Resources.LoggerAddedNewEmployee);
                            }
                            else
                            {
                                context.SaveChanges();
                                int index = Items.IndexOf(SelectedItem);

                                if (index >= 0 && Items.Count > index)
                                {
                                    Items[index] = ItemDetailsVM.ActiveItem;
                                }
                                Logger.Debug(Properties.Resources.LoggerUpdatedEmployee);
                            }

                            transaction.Commit();
                            Logger.Debug(Properties.Resources.LoggerTransactionUpdatedEmployeeExecuted);
                        }
                        catch (Exception e)
                        {
                            Logger.Debug(Properties.Resources.LoggerTransactionUpdatedEmployeeError);
                            Logger.Error(e.StackTrace);
                            transaction.Rollback();
                        }
                    }
                }
            }
        }
    }
}
