using ImpoDoc.Commands;
using ImpoDoc.Common.Logger;
using ImpoDoc.Ioc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Data;

namespace ImpoDoc.ViewModel
{
    public abstract class ItemListViewModel<T> : BaseViewModel
        where T : class, new()
    {
        protected virtual ItemDetailsViewModel<T> ItemDetailsVM => IocKernel.Get<ItemDetailsViewModel<T>>();
        protected abstract void ViewItemDetailsAsync(bool isNew = false);
        protected ILogger Logger;

        protected ItemListViewModel()
        {
            Logger = LoggerFactory.Create<TraceLogger>();
            Items = new ObservableCollection<T>();
            ItemsViewSource = new CollectionViewSource();
            ItemsViewSource.Source = Items;
            FilterText = FilterType = "";
        }

        protected void UpdateItemsViewSource(List<T> items)
        {
            Items = new ObservableCollection<T>(items);
            ItemsViewSource.Source = Items;
            ItemsViewSource.View.Filter += ItemsFilter;
            ItemsViewSource.View.CollectionChanged += View_CollectionChanged;
            Logger.Debug($"Відбулось оновлення даних в колекції");
            UpdateStatusBar();
        }

        protected void View_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Logger.Debug($"Відбулася зміна даних в колекції");
            if (ItemsViewSource == null) return;
            UpdateStatusBar();
        }

        private void UpdateStatusBar()
        {
            StatusText = $"Всього елементів: {(Items != null ? Items.Count : 0)}";
            Logger.Debug($"Відбулось оновлення рядка стану");
        }

        public virtual Dictionary<string, string> FilterList
        {
            get { return GetValue(() => FilterList); }
            set { SetValue(() => FilterList, value); }
        }

        public CollectionViewSource ItemsViewSource
        {
            get { return GetValue(() => ItemsViewSource); }
            set { SetValue(() => ItemsViewSource, value); }
        }

        public string FilterText
        {
            get { return GetValue(() => FilterText); }
            set
            {
                SetValue(() => FilterText, value);
                ItemsViewSource.View.Refresh();
            }
        }

        public string FilterType
        {
            get { return GetValue(() => FilterType); }
            set { SetValue(() => FilterType, value); }
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

        public string StatusText
        {
            get { return GetValue(() => StatusText); }
            set { SetValue(() => StatusText, value); }
        }

        protected bool ItemsFilter(object e)
        {
            if (e is T entity && FilterList.Any(f => f.Key == FilterType) && !string.IsNullOrEmpty(FilterText))
            {
                var value = entity.GetType().GetProperty(FilterType).GetValue(entity, null);
                return value == null || value.ToString().ToUpper().Contains(FilterText.ToUpper());
            }
            else
            {
                return true;
            }
        }

        private RelayCommand<object> createItemCommand;
        public RelayCommand<object> CreateItemCommand
        {
            get
            {
                return createItemCommand ??
                  (createItemCommand = new RelayCommand<object>(obj => ViewItemDetailsAsync(true), CanExecute));
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

        public virtual RelayCommand<object> RemoveItemCommand { get => removeItemCommand; }
        protected RelayCommand<object> removeItemCommand;

        protected bool IsItemSelected(object arg) => SelectedItem != null;
    }
}
