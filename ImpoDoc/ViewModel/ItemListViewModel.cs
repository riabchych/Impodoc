using ImpoDoc.Commands;
using ImpoDoc.Data;
using ImpoDoc.Ioc;
using System.Collections.ObjectModel;

namespace ImpoDoc.ViewModel
{
    public abstract class ItemListViewModel<T> : BaseViewModel
        where T : class, new()
    {
        protected virtual ItemDetailsViewModel<T> ItemDetailsVM { get; }
        protected abstract void ViewItemDetailsAsync(bool isNew = false);

        protected ItemListViewModel()
        {
            ItemDetailsVM = IocKernel.Get<ItemDetailsViewModel<T>>();
            Items = new ObservableCollection<T>();
        }

        public ObservableCollection<T> Items
        {
            get { return GetValue(() => Items); }
            set { SetValue(() => Items, value); }
        }

        public T SelectedItem
        {
            get { return GetValue(() => SelectedItem); }
            set { SetValue(() => SelectedItem, value); }
        }

        private RelayCommand<object> createItemCommand;
        public RelayCommand<object> CreateItemCommand
        {
            get
            {
                return createItemCommand ??
                  (createItemCommand = new RelayCommand<object>(obj => ViewItemDetailsAsync(true), delegate (object arg) { return true; }));
            }
        }

        private RelayCommand<object> viewItemDetailsCommand;
        public RelayCommand<object> ViewItemDetailsCommand
        {
            get
            {
                return viewItemDetailsCommand ??
                  (viewItemDetailsCommand = new RelayCommand<object>(obj => ViewItemDetailsAsync(), IsItemSelected));
            }
        }

        public virtual RelayCommand<object> RemoveItemCommand { get => removeItemCommand;}
        protected RelayCommand<object> removeItemCommand;

        public void Clear(object parameter)
        {
            ItemDetailsVM.ActiveItem = new T();
        }

        protected bool IsItemSelected(object arg) => SelectedItem != null;
    }
}
