using ImpoDoc.Commands;
using ImpoDoc.Common;
using ImpoDoc.Data;
using ImpoDoc.Entities;
using ImpoDoc.Ioc;
using ImpoDoc.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ImpoDoc.ViewModel
{
    public class EmployeeListViewModel : ItemListViewModel<Employee>
    {
        protected override ItemDetailsViewModel<Employee> ItemDetailsVM => IocKernel.Get<EmployeeDetailsViewModel>();
        private EmployeeDetailsWindow ItemDetailsWnd => IocKernel.Get<EmployeeDetailsWindow>();
        public override Dictionary<string, string> FilterList => new Dictionary<string, string>
        {
            { "FirstName",  "Ім'я" },
            { "LastName",  "Прізвище"},
            { "MiddleName", "По батькові" },
            { "Email", "Ел. пошта" },
            { "Department", "Відділ" },
            { "PhoneNumber", "Номер телефону" }
        };

        public async Task LoadDataAsync()
        {
            BusyStatus.Content = "Завантаження списку працівників...";
            using (var context = new DatabaseContext())
            {
                Logger.Debug("Завантаження списку працівників...");
                List<Employee> items = await Task.Run(() => context.Employees.ToListAsync());
                Logger.Debug("Завантаження списку працівників закінчено");
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
                                  Logger.Debug("Виконана транзакція по видаленню працівника");
                              }
                              catch(Exception e)
                              {
                                  Logger.Debug("Транзакція по видаленню працівника закінчилася з помилкою");
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
                                Logger.Debug("Виконано додання нового працівника");
                            }
                            else
                            {
                                context.SaveChanges();
                                int index = Items.IndexOf(SelectedItem);

                                if (index >= 0 && Items.Count > index)
                                {
                                    Items[index] = ItemDetailsVM.ActiveItem;
                                }
                                Logger.Debug("Виконано зміну даних існуючого працівника");
                            }

                            transaction.Commit();
                            Logger.Debug("Виконана транзакція по внесенню даних працівника");
                        }
                        catch (Exception e)
                        {
                            Logger.Debug("Транзакція по внесенню даних працівника закінчилася з помилкою");
                            Logger.Error(e.StackTrace);
                            transaction.Rollback();
                        }
                    }
                }
            }
        }
    }
}
