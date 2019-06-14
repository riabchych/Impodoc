using ImpoDoc.Commands;
using ImpoDoc.Common;
using ImpoDoc.Data;
using ImpoDoc.Entities;
using ImpoDoc.Ioc;
using ImpoDoc.Views.Document.Outgoing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ImpoDoc.ViewModel
{
    public class OutgoingDocListViewModel : ItemListViewModel<OutgoingDocument>
    {
        protected override ItemDetailsViewModel<OutgoingDocument> ItemDetailsVM => IocKernel.Get<OutgoingDocDetailsViewModel>();
        private OutgoingDocDetailsWnd ItemDetailsWnd => IocKernel.Get<OutgoingDocDetailsWnd>();
        public override Dictionary<string, string> FilterList => new Dictionary<string, string>
        {
            { "Name",  "Назва документа" },
            { "OutgoingIndex",  "Індекс документа"},
            { "IncomingIndex", "Індекс кореспондента" },
            { "DocumentType", "Тип документа" },
            { "Description", "Короткий зміст" },
            { "Location", "Місце знаходження" },
        };
        public async Task LoadDataAsync()
        {
            BusyStatus.Content = "Завантаження списку вихідних документів...";
            {
                using (var context = new DatabaseContext())
                {
                    Logger.Debug("Завантаження списку вихідних документів...");
                    List<OutgoingDocument> items = await Task.Run(() => context.OutgoingDocuments
                           .Include(document => document.Attachment)
                           .Include(document => document.Checkout)
                           .Include(document => document.Counter)
                           .Include(document => document.Correspondent)
                           .Include(document => document.Execution)
                               .ThenInclude(execution => execution.Executor)
                           .ToListAsync());
                    UpdateItemsViewSource(items);
                    Logger.Debug("Завантаження списку вихідних документів закінчено");
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
                                  context.Entry(SelectedItem.Correspondent).State = EntityState.Detached;
                                  context.Entry(SelectedItem.Execution.Executor).State = EntityState.Detached;
                                  context.Remove(SelectedItem.Attachment);
                                  context.Remove(SelectedItem.Counter);
                                  context.Remove(SelectedItem.Checkout);
                                  context.Remove(SelectedItem.Execution);
                                  context.OutgoingDocuments.Remove(SelectedItem);
                                  context.SaveChanges();
                                  _ = Items.Remove(SelectedItem);
                                  transaction.Commit();
                                  Logger.Debug("Виконана транзакція по видаленню вихідного документа");
                              }
                              catch (Exception e)
                              {
                                  Logger.Debug("Транзакція по видаленню вихідного документа закінчилася з помилкою");
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
            ItemDetailsVM.ActiveItem = isNew || SelectedItem is null ? new OutgoingDocument() : Utils.CloneObject(SelectedItem);
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
                        ItemDetailsVM.ActiveItem.CorrespondentId = ItemDetailsVM.ActiveItem.Correspondent.Id;
                        ItemDetailsVM.ActiveItem.Execution.ExecutorId = ItemDetailsVM.ActiveItem.Execution.Executor.Id;
                        context.OutgoingDocuments.Attach(ItemDetailsVM.ActiveItem);
                        context.SaveChanges();

                        if (isNew)
                        {
                            Items.Add(ItemDetailsVM.ActiveItem);
                            Logger.Debug("Виконано додання нового вихідного документа");
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
                        Logger.Debug("Виконано зміну даних існуючого вихідного документа");
                    }
                    catch (Exception e)
                    {
                        Logger.Debug("Транзакція по внесенню даних вихідного документа закінчилася з помилкою");
                        Logger.Error(e.StackTrace);
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}
