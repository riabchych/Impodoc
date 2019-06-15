using ImpoDoc.Commands;
using ImpoDoc.Common;
using ImpoDoc.Data;
using ImpoDoc.Entities;
using ImpoDoc.Ioc;
using ImpoDoc.Views.Document.Incoming;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImpoDoc.ViewModel
{
    public class IncomingDocListViewModel : ItemListViewModel<IncomingDocument>
    {
        protected override ItemDetailsViewModel<IncomingDocument> ItemDetailsVM => IocKernel.Get<IncomingDocDetailsViewModel>();
        private IncomingDocDetailsWnd ItemDetailsWnd => IocKernel.Get<IncomingDocDetailsWnd>();

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
            BusyStatus.Content = Properties.Resources.LoadingIncDocList;
            {
                using (var context = new DatabaseContext())
                {
                    Logger.Debug(Properties.Resources.LoadingIncDocList);
                    List<IncomingDocument> items = await Task.Run(() => context.IncomingDocuments
                           .Include(document => document.Attachment)
                           .Include(document => document.Checkout)
                           .Include(document => document.Counter)
                           .Include(document => document.Correspondent)
                           .Include(document => document.Resolution)
                           .Include(document => document.Execution)
                               .ThenInclude(execution => execution.Executor)
                           .ToListAsync());
                    Logger.Debug(Properties.Resources.LoadedIncDocList);
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
                                  Logger.Debug(Properties.Resources.LoggerTransactionRemoveIncDocExecuted);
                              }
                              catch (Exception e)
                              {
                                  Logger.Debug(Properties.Resources.LoggerTransactionRemoveIncDocError);
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
                            Logger.Debug(Properties.Resources.LoggerAddedNewIncDoc);
                        }
                        else
                        {
                            int index = Items.IndexOf(SelectedItem);

                            if (index >= 0 && Items.Count > index)
                            {
                                Items[index] = ItemDetailsVM.ActiveItem;
                            }
                            Logger.Debug(Properties.Resources.LoggerUpdatedIncDoc);
                        }
                        transaction.Commit();
                        Logger.Debug(Properties.Resources.LoggerTransactionUpdatedIncDocExecuted);
                    }
                    catch (Exception e)
                    {
                        Logger.Debug(Properties.Resources.LoggerTransactionUpdatedIncDocError);
                        Logger.Error(e.StackTrace);
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}
