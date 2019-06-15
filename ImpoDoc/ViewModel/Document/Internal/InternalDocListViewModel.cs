using ImpoDoc.Commands;
using ImpoDoc.Common;
using ImpoDoc.Data;
using ImpoDoc.Entities;
using ImpoDoc.Ioc;
using ImpoDoc.Views.Document.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImpoDoc.ViewModel
{
    public class InternalDocListViewModel : ItemListViewModel<InternalDocument>
    {
        protected override ItemDetailsViewModel<InternalDocument> ItemDetailsVM => IocKernel.Get<InternalDocDetailsViewModel>();
        private InternalDocDetailsWnd ItemDetailsWnd => IocKernel.Get<InternalDocDetailsWnd>();

        public override Dictionary<string, string> FilterList => new Dictionary<string, string>
        {
            { "Name",  Properties.Resources.DocumentName },
            { "OutgoingIndex",  Properties.Resources.AddresserIndex },
            { "IncomingIndex", Properties.Resources.AddresseeIndex },
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
                    List<InternalDocument> items = await Task.Run(() => context.InternalDocuments
                           .Include(document => document.Addressee)
                           .Include(document => document.Addresser)
                           .Include(document => document.Attachment)
                           .Include(document => document.Checkout)
                           .Include(document => document.Counter)
                           .Include(document => document.Execution)
                               .ThenInclude(execution => execution.Executor)
                           .ToListAsync());
                    Logger.Debug(Properties.Resources.LoadedIntDocList);
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
                                  Logger.Debug(Properties.Resources.LoggerTransactionRemoveIntDocExecuted);
                              }
                              catch (Exception e)
                              {
                                  Logger.Debug(Properties.Resources.LoggerTransactionRemoveIntDocError);
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
                            Logger.Debug(Properties.Resources.LoggerAddedNewIntDoc);
                        }
                        else
                        {
                            int index = Items.IndexOf(SelectedItem);

                            if (index >= 0 && Items.Count > index)
                            {
                                Items[index] = ItemDetailsVM.ActiveItem;
                            }
                            Logger.Debug(Properties.Resources.LoggerUpdatedIntDoc);

                        }
                        transaction.Commit();
                        Logger.Debug(Properties.Resources.LoggerTransactionUpdatedIntDocExecuted);
                    }
                    catch (Exception e)
                    {
                        Logger.Debug(Properties.Resources.LoggerTransactionUpdatedIntDocError);
                        Logger.Error(e.StackTrace);
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}
