using ImpoDoc.Common;
using ImpoDoc.Data;
using ImpoDoc.Ioc;
using ImpoDoc.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace ImpoDoc
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IocKernel.Initialize(new IocConfiguration());
            base.OnStartup(e);
            _ = LoadContentAsync();
        }

        public async Task LoadContentAsync()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                // context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Database.Migrate();
            }

            BusyStatus.IsBusy = true;
            await IocKernel.Get<EmployeeListViewModel>().LoadDataAsync();
            await IocKernel.Get<CompanyListViewModel>().LoadDataAsync();
            await IocKernel.Get<IncomingDocListViewModel>().LoadDataAsync();
            await IocKernel.Get<OutgoingDocListViewModel>().LoadDataAsync();
            await IocKernel.Get<InternalDocListViewModel>().LoadDataAsync();

            BusyStatus.IsBusy = false;
        }
    }
}
