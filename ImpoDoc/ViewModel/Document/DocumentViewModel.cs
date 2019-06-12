using ImpoDoc.Commands;
using ImpoDoc.Ioc;
using ImpoDoc.Views;

namespace ImpoDoc.ViewModel
{
    public class DocumentViewModel : BaseViewModel
    {
        private const int INCOMING_DOCUMENT = 0;
        private const int OUTGOING_DOCUMENT = 1;
        private const int INTERNAL_DOCUMENT = 2;

        // Document list view models
        public IncomingDocListViewModel IncomingDocListVM => IocKernel.Get<IncomingDocListViewModel>();
        public OutgoingDocListViewModel OutgoingDocListVM => IocKernel.Get<OutgoingDocListViewModel>();
        public InternalDocListViewModel InternalDocListVM => IocKernel.Get<InternalDocListViewModel>();

        // Windows
        private EmployeeListWindow EmployeeListWnd => IocKernel.Get<EmployeeListWindow>();
        private CompanyListWindow CompanyListWnd => IocKernel.Get<CompanyListWindow>();

        // Commands
        private RelayCommand<object> viewEmployeeListCommand;
        public RelayCommand<object> ViewEmployeeListCommand
        {
            get
            {
                return viewEmployeeListCommand ??
                  (viewEmployeeListCommand = new RelayCommand<object>(obj => EmployeeListWnd.ShowDialog(), delegate (object arg) { return true; }));
            }
        }

        private RelayCommand<object> viewCompanyListCommand;
        public RelayCommand<object> ViewCompanyListCommand
        {
            get
            {
                return viewCompanyListCommand ??
                  (viewCompanyListCommand = new RelayCommand<object>(obj => CompanyListWnd.ShowDialog(), delegate (object arg) { return true; }));
            }
        }
        public int CurrentSectionIndex
        {
            get { return GetValue(() => CurrentSectionIndex); }
            set { SetValue(() => CurrentSectionIndex, value); }
        }

        private RelayCommand<object> _createItemCommand;
        public RelayCommand<object> CreateItemCommand
        {
            get
            {
                return _createItemCommand ?? (_createItemCommand = new RelayCommand<object>(obj =>
                  {
                      switch (CurrentSectionIndex)
                      {
                          case OUTGOING_DOCUMENT:
                              OutgoingDocListVM.CreateItemCommand.Execute(true);
                              break;
                          case INTERNAL_DOCUMENT:
                              InternalDocListVM.CreateItemCommand.Execute(true);
                              break;
                          default:
                              IncomingDocListVM.CreateItemCommand.Execute(true);
                              break;
                      }
                  }, delegate (object arg) { return true; }));
            }
        }

        private RelayCommand<object> _viewItemDetailsCommand;
        public RelayCommand<object> ViewItemDetailsCommand
        {
            get
            {
                return _viewItemDetailsCommand ?? (_viewItemDetailsCommand = new RelayCommand<object>(obj =>
                {
                    switch (CurrentSectionIndex)
                    {
                        case OUTGOING_DOCUMENT:
                            OutgoingDocListVM.ViewItemDetailsCommand.Execute(true);
                            break;
                        case INTERNAL_DOCUMENT:
                            InternalDocListVM.ViewItemDetailsCommand.Execute(true);
                            break;
                        default:
                            IncomingDocListVM.ViewItemDetailsCommand.Execute(true);
                            break;
                    }
                }, delegate (object arg) { return true; }));
            }
        }

        private RelayCommand<object> _removeItemCommand;
        public RelayCommand<object> RemoveItemCommand
        {
            get
            {
                return _removeItemCommand ?? (_removeItemCommand = new RelayCommand<object>(obj =>
                {
                    switch (CurrentSectionIndex)
                    {
                        case OUTGOING_DOCUMENT:
                            OutgoingDocListVM.RemoveItemCommand.Execute(true);
                            break;
                        case INTERNAL_DOCUMENT:
                            InternalDocListVM.RemoveItemCommand.Execute(true);
                            break;
                        default:
                            IncomingDocListVM.RemoveItemCommand.Execute(true);
                            break;
                    }
                }, delegate (object arg) { return true; }));
            }
        }
    }
}
