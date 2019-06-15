using ImpoDoc.Commands;
using System.Windows;

namespace ImpoDoc.ViewModel
{
    public class ItemDetailsViewModel<T> : BaseViewModel where T : class, new()
    {
        public ItemDetailsViewModel() { }

        public T ActiveItem
        {
            get { return GetValue(() => ActiveItem); }
            set { SetValue(() => ActiveItem, value); }
        }

        private RelayCommand<object> saveCommand;
        public RelayCommand<object> SaveCommand
        {
            get
            {
                return saveCommand ??
                  (saveCommand = new RelayCommand<object>(obj =>
                  {
                      if (obj is Window window)
                      {
                          window.DialogResult = true;
                      }
                  }, CanSave));
            }
        }

        private RelayCommand<object> closeCommand;
        public RelayCommand<object> CloseCommand
        {
            get
            {
                return closeCommand ??
                  (closeCommand = new RelayCommand<object>(obj =>
                  {
                      if (obj is Window window)
                      {
                          window.DialogResult = false;
                      }
                  }));
            }
        }

        private bool CanSave(object param)
        {
            return !HasError();
        }


    }
}
