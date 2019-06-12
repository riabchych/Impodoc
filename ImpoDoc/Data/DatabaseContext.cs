using ImpoDoc.Entities;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace ImpoDoc.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Resolution> Resolutions { get; set; }
        public DbSet<Checkout> Checkouts { get; set; }
        public DbSet<Counter> Counters { get; set; }
        public DbSet<Execution> Executions { get; set; }

        public DbSet<IncomingDocument> IncomingDocuments { get; set; }
        public DbSet<OutgoingDocument> OutgoingDocuments { get; set; }
        public DbSet<InternalDocument> InternalDocuments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConfigurationManager.ConnectionStrings["CoreDatabase"].ConnectionString);
        }
    }
}
