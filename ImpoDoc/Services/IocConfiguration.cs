using ImpoDoc.ViewModel;
using Ninject.Modules;
using ImpoDoc.Views;

namespace ImpoDoc.Services
{
    class IocConfiguration : NinjectModule
    {
        public override void Load()
        {
            Bind<IEmployeeDataService>().To<EmployeeDataService>().InSingletonScope(); // Reuse same storage every time
            Bind<ListEmployeeViewModel>().ToSelf().InSingletonScope();
            Bind<SingleEmployeeViewModel>().ToSelf().InSingletonScope();

            Bind<EmployeeCardWindow>().ToSelf().InTransientScope(); // Create new instance every time
            Bind<EmployeeListWindow>().ToSelf().InTransientScope(); // Create new instance every time
        }
    }
}
