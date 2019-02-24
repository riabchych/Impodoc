using ImpoDoc.Services;

namespace ImpoDoc.ViewModel
{
    public class ViewModelLocator
    {
        public ListEmployeeViewModel ListEmployeeVM
        {
            get { return IocKernel.Get<ListEmployeeViewModel>(); } // Loading UserControlViewModel will automatically load the binding for IStorage
        }

        public SingleEmployeeViewModel SingleEmployeeVM
        {
            get { return IocKernel.Get<SingleEmployeeViewModel>(); } // Loading UserControlViewModel will automatically load the binding for IStorage
        }
    }
}