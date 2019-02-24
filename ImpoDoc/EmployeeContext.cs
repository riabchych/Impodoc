using ImpoDoc.Models;
using Microsoft.EntityFrameworkCore;

namespace ImpoDoc
{
    class EmployeeContext: DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("employees");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           optionsBuilder.UseSqlite("Data Source=employee.db");
        }
    }
}
