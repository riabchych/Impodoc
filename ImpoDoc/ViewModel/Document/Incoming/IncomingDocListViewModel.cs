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

namespace ImpoDoc.ViewModel
{
    public class IncomingDocListViewModel : ItemListViewModel<IncomingDocument>
    {
        protected override ItemDetailsViewModel<IncomingDocument> ItemDetailsVM => IocKernel.Get<IncomingDocDetailsViewModel>();
        private IncomingDocDetailsWnd ItemDetailsWnd => IocKernel.Get<IncomingDocDetailsWnd>();

        public async Task LoadDataAsync()
        {
            BusyStatus.Content = "Завантаження списку вхідних документів...";
            {
                using (var context = new DatabaseContext())
                {
                    List<IncomingDocument> result = await Task.Run(() => context.IncomingDocuments
                        .Include(document => document.Attachment)
                        .Include(document => document.Checkout)
                        .Include(document => document.Counter)
                        .Include(document => document.Correspondent)
                        .Include(document => document.Execution)
                        .Include(document => document.Resolution)
                        .ToListAsync());
                    Items = new ObservableCollection<IncomingDocument>(result);
                }
            }
        }
        protected override async void ViewItemDetailsAsync(bool isNew = false)
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
                        Company correspondent = ItemDetailsVM.ActiveItem.Correspondent;
                        Employee executor = ItemDetailsVM.ActiveItem.Execution.Executor;
                        ItemDetailsVM.ActiveItem.Correspondent = null;
                        ItemDetailsVM.ActiveItem.Execution.Executor = null;

                        if (isNew)
                        {
                            context.IncomingDocuments.Add(ItemDetailsVM.ActiveItem);
                            await context.SaveChangesAsync();
                            ItemDetailsVM.ActiveItem.Correspondent = correspondent;
                            ItemDetailsVM.ActiveItem.Execution.Executor = executor;
                            Items.Add(ItemDetailsVM.ActiveItem);
                        }
                        else
                        {
                            context.IncomingDocuments.Update(ItemDetailsVM.ActiveItem);
                            await context.SaveChangesAsync();
                            ItemDetailsVM.ActiveItem.Correspondent = correspondent;
                            ItemDetailsVM.ActiveItem.Execution.Executor = executor;
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
