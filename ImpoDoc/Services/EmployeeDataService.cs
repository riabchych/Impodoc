using ImpoDoc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImpoDoc.Services
{
    internal class EmployeeDataService : IEmployeeDataService
    {
        private bool IsInitialized;

        #region Constructor
        public EmployeeDataService() { }
        #endregion

        #region Initialization
        private EmployeeContext Init(EmployeeContext Db)
        {
            if (IsInitialized) return Db;
            //Create the database file at a path defined in SimpleDataStorage
            Db.Database.EnsureCreated();
            //Create the database tables defined in SimpleDataStorage
            Db.Database.Migrate();
            IsInitialized = true;
            return Db;
        }
        #endregion

        #region Add employee
        public void Add(Employee employee)
        {
            try
            {
                BusyStatus.IsBusy = true;
                BusyStatus.Content = Properties.Resources.DataAdding;

                using (EmployeeContext Db = new EmployeeContext())
                {
                    var strategy = Init(Db).Database.CreateExecutionStrategy();
                    Db.Employees.Add(employee);

                    strategy.ExecuteInTransaction(Db,
                        operation: async context =>
                        {
                            await context.SaveChangesAsync(acceptAllChangesOnSuccess: false);
                        },
                        verifySucceeded: context => context.Employees.AsNoTracking().Any(e => e.Id == employee.Id));

                    Db.ChangeTracker.AcceptAllChanges();
                }
            }
            finally
            {
                BusyStatus.IsBusy = false;
                BusyStatus.Content = Properties.Resources.Done;
            }
        }
        #endregion

        #region Update employee
        public void Update(Employee employee)
        {
            try
            {
                BusyStatus.IsBusy = true;
                BusyStatus.Content = Properties.Resources.DataUpdating;

                using (EmployeeContext Db = new EmployeeContext())
                {
                    var strategy = Init(Db).Database.CreateExecutionStrategy();
                    Db.Employees.Update(employee);

                    strategy.ExecuteInTransaction(Db,
                        operation: async context =>
                        {
                            await context.SaveChangesAsync(acceptAllChangesOnSuccess: false);
                        },
                        verifySucceeded: context => context.Employees.AsNoTracking().Any(e => e.Id == employee.Id));

                    Db.ChangeTracker.AcceptAllChanges();
                }
            }
            finally
            {
                BusyStatus.IsBusy = false;
                BusyStatus.Content = Properties.Resources.Done;
            }
        }
        #endregion

        #region Remove employee
        public void Remove(Employee employee)
        {
            try
            {
                BusyStatus.IsBusy = true;
                BusyStatus.Content = Properties.Resources.DataDeleting;
                using (EmployeeContext Db = new EmployeeContext())
                {
                    var strategy = Init(Db).Database.CreateExecutionStrategy();

                    strategy.Execute(async () =>
                    {
                        using (var context = new EmployeeContext())
                        {
                            using (var transaction = context.Database.BeginTransaction())
                            {
                                context.Employees.Remove(employee);
                                await context.SaveChangesAsync();
                                transaction.Commit();
                            }
                        }
                    });
                }
            }
            finally
            {
                BusyStatus.IsBusy = false;
                BusyStatus.Content = Properties.Resources.Done;
            }

        }
        #endregion

        #region Load employees async
        public async Task<List<Employee>> LoadAsync()
        {
            try
            {
                using (EmployeeContext Db = new EmployeeContext())
                {
                    BusyStatus.IsBusy = true;
                    BusyStatus.Content = Properties.Resources.DataLoading;
                    var result = await Init(Db).Employees.ToListAsync();
                    return result;
                }
            }
            finally
            {
                BusyStatus.IsBusy = false;
                BusyStatus.Content = Properties.Resources.Done;
            }
        }
        #endregion
    }
}
