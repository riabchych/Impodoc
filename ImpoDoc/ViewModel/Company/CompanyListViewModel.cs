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
    public class CompanyListViewModel : ItemListViewModel<Company>
    {
        protected override ItemDetailsViewModel<Company> ItemDetailsVM => IocKernel.Get<CompanyDetailsViewModel>();
        private CompanyDetailsWindow ItemDetailsWnd => IocKernel.Get<CompanyDetailsWindow>();
        public override Dictionary<string, string> FilterList => new Dictionary<string, string>
        {
            { "Title",  "Назва" },
            { "Location",  "Місто" },
            { "LegalAddress", "Адреса" },
            { "INN", "ЄДРПОУ" },
            { "PhoneNumber", "Номер телефону" },
            { "Director", "Керівник" }
        };

        public async Task LoadDataAsync()
        {
            BusyStatus.Content = "Завантаження списку організацій...";
            using (var context = new DatabaseContext())
            {
                Logger.Debug("Завантаження списку організацій...");
                List<Company> items = await Task.Run(() => context.Companies.ToListAsync());
                Logger.Debug("Завантаження списку організацій закінчено");
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
                                  context.Companies.Remove(SelectedItem);
                                  context.SaveChanges();
                                  _ = Items.Remove(SelectedItem);
                                  transaction.Commit();
                                  Logger.Debug("Виконана транзакція по видаленню організації");
                              }
                              catch(Exception e)
                              {
                                  Logger.Debug("Транзакція по видаленню організації закінчилася з помилкою");
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
            ItemDetailsVM.ActiveItem = isNew || SelectedItem is null ? new Company() : Utils.CloneObject(SelectedItem);

            if (ItemDetailsWnd.ShowDialog() == true)
            {
                using (var context = new DatabaseContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            context.Companies.Attach(ItemDetailsVM.ActiveItem);
                            context.SaveChanges();

                            if (isNew)
                            {
                                Items.Add(ItemDetailsVM.ActiveItem);
                                Logger.Debug("Виконано додання нової організації");

                            }
                            else
                            {
                                int index = Items.IndexOf(SelectedItem);

                                if (index >= 0 && Items.Count > index)
                                {
                                    Items[index] = ItemDetailsVM.ActiveItem;
                                }
                                Logger.Debug("Виконано зміну даних існуючої організації");
                            }

                            transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            Logger.Debug("Транзакція по внесенню даних організації закінчилася з помилкою");
                            Logger.Error(e.StackTrace);
                            transaction.Rollback();
                        }
                    }
                }
            }
        }
    }
}
