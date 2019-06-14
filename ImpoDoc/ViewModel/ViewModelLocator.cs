using ImpoDoc.Ioc;

namespace ImpoDoc.ViewModel
{
    public class ViewModelLocator
    {
        // Employee

        public EmployeeListViewModel EmployeeListVM
        {
            get { return IocKernel.Get<EmployeeListViewModel>(); }
        }

        public EmployeeDetailsViewModel EmployeeDetailsVM
        {
            get { return IocKernel.Get<EmployeeDetailsViewModel>(); }
        }

        // Company

        public CompanyListViewModel CompanyListVM
        {
            get { return IocKernel.Get<CompanyListViewModel>(); }
        }

        public CompanyDetailsViewModel CompanyDetailsVM
        {
            get { return IocKernel.Get<CompanyDetailsViewModel>(); }
        }

        // Document

        public IncomingDocListViewModel IncomingDocListVM
        {
            get { return IocKernel.Get<IncomingDocListViewModel>(); }
        }

        public InternalDocListViewModel InternalDocListVM
        {
            get { return IocKernel.Get<InternalDocListViewModel>(); }
        }

        public OutgoingDocListViewModel OutgoingDocListVM
        {
            get { return IocKernel.Get<OutgoingDocListViewModel>(); }
        }

        public IncomingDocDetailsViewModel IncomingDocDetailsVM
        {
            get { return IocKernel.Get<IncomingDocDetailsViewModel>(); }
        }

        public InternalDocDetailsViewModel InternalDocDetailsVM
        {
            get { return IocKernel.Get<InternalDocDetailsViewModel>(); }
        }

        public OutgoingDocDetailsViewModel OutgoingDocDetailsVM
        {
            get { return IocKernel.Get<OutgoingDocDetailsViewModel>(); }
        }

        public DocumentViewModel DocumentVM
        {
            get { return IocKernel.Get<DocumentViewModel>(); }
        }
    }
}