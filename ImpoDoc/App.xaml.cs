using ImpoDoc.Common;
using ImpoDoc.Common.Logger;
using ImpoDoc.Ioc;
using ImpoDoc.ViewModel;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace ImpoDoc
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ILogger Logger = LoggerFactory.Create<TraceLogger>();

        protected override void OnStartup(StartupEventArgs e)
        {
            IocKernel.Initialize(new IocConfiguration());
            base.OnStartup(e);
            _ = LoadContentAsync();
        }

        public async Task LoadContentAsync()
        {
            BusyStatus.IsBusy = true;
            try
            {
                await IocKernel.Get<EmployeeListViewModel>().LoadDataAsync();
                await IocKernel.Get<CompanyListViewModel>().LoadDataAsync();
                await IocKernel.Get<IncomingDocListViewModel>().LoadDataAsync();
                await IocKernel.Get<OutgoingDocListViewModel>().LoadDataAsync();
                await IocKernel.Get<InternalDocListViewModel>().LoadDataAsync();
            }
            catch (Exception e)
            {
                Logger.Debug(e.StackTrace);
            }
            finally
            {
                BusyStatus.IsBusy = false;
            }
        }
    }
}
