﻿using ImpoDoc.ViewModel;
using Ninject.Modules;
using ImpoDoc.Views;
using ImpoDoc.Common;
using ImpoDoc.Common.Logger;

namespace ImpoDoc.Ioc
{
    public class IocConfiguration : NinjectModule
    {
        private readonly ILogger Logger;

        public IocConfiguration()
        {
            Logger = LoggerFactory.Create<TraceLogger>();

        }

        public override void Load()
        {
            // Employee
            Bind<EmployeeListViewModel>().ToSelf().InSingletonScope();
            Bind<EmployeeDetailsViewModel>().ToSelf().InSingletonScope();

            Bind<EmployeeDetailsWindow>().ToSelf().InTransientScope();
            Bind<EmployeeListWindow>().ToSelf().InTransientScope();

            // Company
            Bind<CompanyListViewModel>().ToSelf().InSingletonScope();
            Bind<CompanyDetailsViewModel>().ToSelf().InSingletonScope();

            Bind<CompanyDetailsWindow>().ToSelf().InTransientScope();
            Bind<CompanyListWindow>().ToSelf().InTransientScope();

            // Document
            Bind<IncomingDocListViewModel>().ToSelf().InSingletonScope();
            Bind<IncomingDocDetailsViewModel>().ToSelf().InSingletonScope();
            Bind<InternalDocListViewModel>().ToSelf().InSingletonScope();
            Bind<InternalDocDetailsViewModel>().ToSelf().InSingletonScope();
            Bind<OutgoingDocListViewModel>().ToSelf().InSingletonScope();
            Bind<OutgoingDocDetailsViewModel>().ToSelf().InSingletonScope();
            Bind<DocumentViewModel>().ToSelf().InSingletonScope();

            Logger.Debug("Відбулось зв'язування IoC");
        }
    }
}
