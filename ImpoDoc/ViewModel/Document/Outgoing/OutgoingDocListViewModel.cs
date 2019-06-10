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

        public async Task LoadDataAsync()
        {
            BusyStatus.Content = "Завантаження списку вихідних документів...";
            {
                using (var context = new DatabaseContext())
                {
                    List<OutgoingDocument> result = await Task.Run(() => context.OutgoingDocuments
                        .Include(document => document.Attachment)
                        .Include(document => document.Checkout)
                        .Include(document => document.Counter)
                        .Include(document => document.Correspondent)
                        .Include(document => document.Execution)
                        .ToListAsync());
                    Items = new ObservableCollection<OutgoingDocument>(result);
                }
            }
        }
        protected override async void ViewItemDetailsAsync(bool isNew = false)
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
                        Company correspondent = ItemDetailsVM.ActiveItem.Correspondent;
                        Employee executor = ItemDetailsVM.ActiveItem.Execution.Executor;
                        ItemDetailsVM.ActiveItem.Correspondent = null;
                        ItemDetailsVM.ActiveItem.Execution.Executor = null;

                        if (isNew)
                        {
                            context.OutgoingDocuments.Add(ItemDetailsVM.ActiveItem);
                            await context.SaveChangesAsync();
                            ItemDetailsVM.ActiveItem.Correspondent = correspondent;
                            ItemDetailsVM.ActiveItem.Execution.Executor = executor;
                            Items.Add(ItemDetailsVM.ActiveItem);
                        }
                        else
                        {
                            context.OutgoingDocuments.Update(ItemDetailsVM.ActiveItem);
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
