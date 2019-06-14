using ImpoDoc.Commands;
using ImpoDoc.Common;
using ImpoDoc.Data;
using ImpoDoc.Entities;
using ImpoDoc.Ioc;
using ImpoDoc.Views.Document.Incoming;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;

namespace ImpoDoc.ViewModel
{
    public class IncomingDocListViewModel : ItemListViewModel<IncomingDocument>
    {
        protected override ItemDetailsViewModel<IncomingDocument> ItemDetailsVM => IocKernel.Get<IncomingDocDetailsViewModel>();
        private IncomingDocDetailsWnd ItemDetailsWnd => IocKernel.Get<IncomingDocDetailsWnd>();

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
            BusyStatus.Content = "Завантаження списку вхідних документів...";
            {
                using (var context = new DatabaseContext())
                {
                    Logger.Debug("Завантаження списку вхідних документів...");
                    List<IncomingDocument> items = await Task.Run(() => context.IncomingDocuments
                           .Include(document => document.Attachment)
                           .Include(document => document.Checkout)
                           .Include(document => document.Counter)
                           .Include(document => document.Correspondent)
                           .Include(document => document.Resolution)
                           .Include(document => document.Execution)
                               .ThenInclude(execution => execution.Executor)
                           .ToListAsync());
                    Logger.Debug("Завантаження списку вхідних документів закінчено");
                    UpdateItemsViewSource(items);
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
                                  context.Remove(SelectedItem.Resolution);
                                  context.IncomingDocuments.Remove(SelectedItem);
                                  context.SaveChanges();
                                  _ = Items.Remove(SelectedItem);
                                  transaction.Commit();
                                  Logger.Debug("Виконана транзакція по видаленню вхідного документа");
                              }
                              catch (Exception e)
                              {
                                  Logger.Debug("Транзакція по видаленню вхідного документа закінчилася з помилкою");
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
            ItemDetailsVM.ActiveItem = isNew || SelectedItem is null ? new IncomingDocument() : Utils.CloneObject(SelectedItem);
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
                        context.IncomingDocuments.Attach(ItemDetailsVM.ActiveItem);
                        context.SaveChanges();

                        if (isNew)
                        {
                            Items.Add(ItemDetailsVM.ActiveItem);
                            Logger.Debug("Виконано додання нового вхідного документа");

                        }
                        else
                        {
                            int index = Items.IndexOf(SelectedItem);

                            if (index >= 0 && Items.Count > index)
                            {
                                Items[index] = ItemDetailsVM.ActiveItem;
                            }
                            Logger.Debug("Виконано зміну даних існуючого вхідного документа");
                        }
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Logger.Debug("Транзакція по внесенню даних вхідного документа закінчилася з помилкою");
                        Logger.Error(e.StackTrace);
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}
