using ImpoDoc.Commands;
using ImpoDoc.Common;
using ImpoDoc.Data;
using ImpoDoc.Entities;
using ImpoDoc.Ioc;
using ImpoDoc.Views.Document.Outgoing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImpoDoc.ViewModel
{
    public class OutgoingDocListViewModel : ItemListViewModel<OutgoingDocument>
    {
        protected override ItemDetailsViewModel<OutgoingDocument> ItemDetailsVM => IocKernel.Get<OutgoingDocDetailsViewModel>();
        private OutgoingDocDetailsWnd ItemDetailsWnd => IocKernel.Get<OutgoingDocDetailsWnd>();

        public override Dictionary<string, string> FilterList => new Dictionary<string, string>
        {
            { "Name",  Properties.Resources.DocumentName },
            { "OutgoingIndex",  Properties.Resources.DocumentOutgoingIndex},
            { "IncomingIndex", Properties.Resources.DocumentIncomingIndex},
            { "DocumentType", Properties.Resources.DocumentType },
            { "Description", Properties.Resources.DocumentDescription },
            { "Location", Properties.Resources.DocumentLocation },
        };
        public async Task LoadDataAsync()
        {
            BusyStatus.Content = Properties.Resources.LoadingOutDocList;
            {
                using (var context = new DatabaseContext())
                {
                    Logger.Debug(Properties.Resources.LoadingOutDocList);
                    List<OutgoingDocument> items = await Task.Run(() => context.OutgoingDocuments
                           .Include(document => document.Attachment)
                           .Include(document => document.Checkout)
                           .Include(document => document.Counter)
                           .Include(document => document.Correspondent)
                           .Include(document => document.Execution)
                               .ThenInclude(execution => execution.Executor)
                           .ToListAsync());
                    UpdateItemsViewSource(items);
                    Logger.Debug(Properties.Resources.LoadedOutDocList);
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
                                  Logger.Debug(Properties.Resources.LoggerTransactionRemoveOutDocExecuted);
                              }
                              catch (Exception e)
                              {
                                  Logger.Debug(Properties.Resources.LoggerTransactionRemoveOutDocError);
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
                            Logger.Debug(Properties.Resources.LoggerAddedNewOutDoc);
                        }
                        else
                        {
                            int index = Items.IndexOf(SelectedItem);

                            if (index >= 0 && Items.Count > index)
                            {
                                Items[index] = ItemDetailsVM.ActiveItem;
                            }
                            Logger.Debug(Properties.Resources.LoggerUpdatedOutDoc);
                        }
                        transaction.Commit();
                        Logger.Debug(Properties.Resources.LoggerTransactionUpdatedOutDocExecuted);
                    }
                    catch (Exception e)
                    {
                        Logger.Debug(Properties.Resources.LoggerTransactionUpdatedOutDocError);
                        Logger.Error(e.StackTrace);
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}
