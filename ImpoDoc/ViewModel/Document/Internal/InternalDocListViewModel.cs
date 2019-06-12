using ImpoDoc.Commands;
using ImpoDoc.Common;
using ImpoDoc.Data;
using ImpoDoc.Entities;
using ImpoDoc.Ioc;
using ImpoDoc.Views.Document.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ImpoDoc.ViewModel
{
    public class InternalDocListViewModel : ItemListViewModel<InternalDocument>
    {
        protected override ItemDetailsViewModel<InternalDocument> ItemDetailsVM => IocKernel.Get<InternalDocDetailsViewModel>();
        private InternalDocDetailsWnd ItemDetailsWnd => IocKernel.Get<InternalDocDetailsWnd>();

        public async Task LoadDataAsync()
        {
            BusyStatus.Content = "Завантаження списку внутрішніх документів...";
            {
                using (var context = new DatabaseContext())
                {
                    List<InternalDocument> result = await Task.Run(() => context.InternalDocuments
                        .Include(document => document.Addressee)
                        .Include(document => document.Addresser)
                        .Include(document => document.Attachment)
                        .Include(document => document.Checkout)
                        .Include(document => document.Counter)
                        .Include(document => document.Execution)
                            .ThenInclude(execution => execution.Executor)
                        .ToListAsync());
                    Items = new ObservableCollection<InternalDocument>(result);
                }
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
                                  context.Entry(SelectedItem.Addressee).State = EntityState.Detached;
                                  context.Entry(SelectedItem.Addresser).State = EntityState.Detached;
                                  context.Entry(SelectedItem.Execution.Executor).State = EntityState.Detached;
                                  context.Remove(SelectedItem.Attachment);
                                  context.Remove(SelectedItem.Counter);
                                  context.Remove(SelectedItem.Checkout);
                                  context.Remove(SelectedItem.Execution);
                                  context.InternalDocuments.Remove(SelectedItem);
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
            ItemDetailsVM.ActiveItem = isNew || SelectedItem is null ? new InternalDocument() : Utils.CloneObject(SelectedItem);
            bool? dialogOpenResult = ItemDetailsWnd.ShowDialog();

            if (dialogOpenResult != true)
            {
                return;
            }

            using (var context = new DatabaseContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        ItemDetailsVM.ActiveItem.AddresseeId = ItemDetailsVM.ActiveItem.Addressee.Id;
                        ItemDetailsVM.ActiveItem.AddresserId = ItemDetailsVM.ActiveItem.Addresser.Id;
                        ItemDetailsVM.ActiveItem.Execution.ExecutorId = ItemDetailsVM.ActiveItem.Execution.Executor.Id;
                        context.InternalDocuments.Attach(ItemDetailsVM.ActiveItem);
                        context.SaveChanges();

                        if (isNew)
                        {
                            Items.Add(ItemDetailsVM.ActiveItem);
                        }
                        else
                        {
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
