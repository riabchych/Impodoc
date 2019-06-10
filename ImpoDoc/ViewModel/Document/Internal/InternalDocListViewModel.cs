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
                        .ToListAsync());
                    Items = new ObservableCollection<InternalDocument>(result);
                }
            }
        }
        protected override async void ViewItemDetailsAsync(bool isNew = false)
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
                        Employee addressee = ItemDetailsVM.ActiveItem.Addressee;
                        Employee addresser = ItemDetailsVM.ActiveItem.Addresser;
                        Employee executor = ItemDetailsVM.ActiveItem.Execution.Executor;
                        ItemDetailsVM.ActiveItem.Addressee = null;
                        ItemDetailsVM.ActiveItem.Addresser = null;
                        ItemDetailsVM.ActiveItem.Execution.Executor = null;

                        if (isNew)
                        {
                            context.InternalDocuments.Add(ItemDetailsVM.ActiveItem);
                            await context.SaveChangesAsync();
                            ItemDetailsVM.ActiveItem.Addressee = addressee;
                            ItemDetailsVM.ActiveItem.Addresser = addresser;
                            ItemDetailsVM.ActiveItem.Execution.Executor = executor;
                            Items.Add(ItemDetailsVM.ActiveItem);
                        }
                        else
                        {
                            context.InternalDocuments.Update(ItemDetailsVM.ActiveItem);
                            await context.SaveChangesAsync();
                            ItemDetailsVM.ActiveItem.Addressee = addressee;
                            ItemDetailsVM.ActiveItem.Addresser = addresser;
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
