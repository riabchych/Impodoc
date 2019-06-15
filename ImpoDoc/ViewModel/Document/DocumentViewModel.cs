using ImpoDoc.Commands;
using ImpoDoc.Common;
using ImpoDoc.Ioc;
using ImpoDoc.Views;

namespace ImpoDoc.ViewModel
{
    public class DocumentViewModel : BaseViewModel
    {
        public IncomingDocListViewModel IncomingDocListVM => IocKernel.Get<IncomingDocListViewModel>();
        public OutgoingDocListViewModel OutgoingDocListVM => IocKernel.Get<OutgoingDocListViewModel>();
        public InternalDocListViewModel InternalDocListVM => IocKernel.Get<InternalDocListViewModel>();

        private EmployeeListWindow EmployeeListWnd => IocKernel.Get<EmployeeListWindow>();
        private CompanyListWindow CompanyListWnd => IocKernel.Get<CompanyListWindow>();
        private object GetCurrentVM() => GetType().GetProperty(CurrentSection).GetValue(this, null);

        public DocumentViewModel()
        {
            CurrentSection = "IncomingDocListVM";
            CurrentVM = GetCurrentVM() ;
        }

        public object CurrentVM
        {
            get { return GetValue(() => CurrentVM); }
            set { SetValue(() => CurrentVM, value); }
        }

        public string CurrentSection
        {
            get { return GetValue(() => CurrentSection); }
            set
            {
                SetValue(() => CurrentSection, value);
                CurrentVM = GetCurrentVM();
            }
        }

        // Commands
        private RelayCommand<object> viewEmployeeListCommand;
        public RelayCommand<object> ViewEmployeeListCommand
        {
            get
            {
                return viewEmployeeListCommand ??
                  (viewEmployeeListCommand = new RelayCommand<object>(obj => EmployeeListWnd.ShowDialog(), CanExecute));
            }
        }

        private RelayCommand<object> viewCompanyListCommand;
        public RelayCommand<object> ViewCompanyListCommand
        {
            get
            {
                return viewCompanyListCommand ??
                  (viewCompanyListCommand = new RelayCommand<object>(obj => CompanyListWnd.ShowDialog(), CanExecute));
            }
        }

        private RelayCommand<object> _createItemCommand;
        public RelayCommand<object> CreateItemCommand
        {
            get
            {
                return _createItemCommand ?? (_createItemCommand = new RelayCommand<object>(obj =>
                  {
                      ((RelayCommand<object>)CurrentVM.GetType().GetProperty("CreateItemCommand").GetValue(CurrentVM, null)).Execute(true);
                  }, CanExecute));
            }
        }

        private RelayCommand<object> _viewItemDetailsCommand;
        public RelayCommand<object> ViewItemDetailsCommand
        {
            get
            {
                return _viewItemDetailsCommand ?? (_viewItemDetailsCommand = new RelayCommand<object>(obj =>
                {
                    ((RelayCommand<object>)CurrentVM.GetType().GetProperty("ViewItemDetailsCommand").GetValue(CurrentVM, null)).Execute(true);
                }, CanExecute));
            }
        }

        private RelayCommand<object> _removeItemCommand;
        public RelayCommand<object> RemoveItemCommand
        {
            get
            {
                return _removeItemCommand ?? (_removeItemCommand = new RelayCommand<object>(obj =>
                {
                    ((RelayCommand<object>)CurrentVM.GetType().GetProperty("RemoveItemCommand").GetValue(CurrentVM, null)).Execute(true);
                }, CanExecute));
            }
        }


        

        private RelayCommand<object> _viewAboutWindowCommand;
        public RelayCommand<object> ViewAboutWindowCommand
        {
            get
            {
                return _viewAboutWindowCommand ?? (_viewAboutWindowCommand = new RelayCommand<object>(obj =>
                {
                    About about = new About();
                    about.Show();
                }, CanExecute));
            }
        }
    }
}
