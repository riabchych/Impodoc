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
    public class CompanyListViewModel : ItemListViewModel<Company>
    {
        protected override ItemDetailsViewModel<Company> ItemDetailsVM => IocKernel.Get<CompanyDetailsViewModel>();
        private CompanyDetailsWindow ItemDetailsWnd => IocKernel.Get<CompanyDetailsWindow>();

        public async Task LoadDataAsync()
        {
            BusyStatus.Content = "Завантаження списку компаній...";
            using (var context = new DatabaseContext())
            {
                List<Company> result = await Task.Run(() => context.Companies.ToListAsync());
                Items = new ObservableCollection<Company>(result);
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
                                  context.Companies.Remove(SelectedItem);
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

        protected override async void ViewItemDetailsAsync(bool isNew = false)
        {
            ItemDetailsVM.ActiveItem = isNew || SelectedItem is null ? new Company() : Utils.CloneObject(SelectedItem);

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
                                context.Companies.Add(ItemDetailsVM.ActiveItem);
                                await context.SaveChangesAsync();
                                Items.Add(ItemDetailsVM.ActiveItem);
                            }
                            else
                            {
                                context.Companies.Update(ItemDetailsVM.ActiveItem);
                                await context.SaveChangesAsync();
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
