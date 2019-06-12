using ImpoDoc.Commands;
using ImpoDoc.Common;
using ImpoDoc.Data;
using ImpoDoc.Entities;
using ImpoDoc.Ioc;
using ImpoDoc.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ImpoDoc.ViewModel
{
    public class EmployeeListViewModel : ItemListViewModel<Employee>
    {
        protected override ItemDetailsViewModel<Employee> ItemDetailsVM => IocKernel.Get<EmployeeDetailsViewModel>();
        private EmployeeDetailsWindow ItemDetailsWnd => IocKernel.Get<EmployeeDetailsWindow>();

        public async Task LoadDataAsync()
        {
            BusyStatus.Content = "Завантаження списку працівників...";
            using (var context = new DatabaseContext())
            {
                List<Employee> result = await Task.Run(() => context.Employees.ToListAsync());
                Items = new ObservableCollection<Employee>(result);
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
                              }
                              catch
                              {
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
                            }
                            else
                            {
                                context.SaveChanges();
                                int index = Items.IndexOf(SelectedItem);

                                if (index >= 0 && Items.Count > index)
                                {
                                    Items[index] = ItemDetailsVM.ActiveItem;
                                }
                            }

                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
        }
    }
}
