using ImpoDoc.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImpoDoc.Data
{
    public class DatabaseContext : DbContext
    {
        private string FileName => "core.db";
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<IncomingDocument> IncomingDocuments { get; set; }
        public DbSet<OutgoingDocument> OutgoingDocuments { get; set; }
        public DbSet<InternalDocument> InternalDocuments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Attachments
            modelBuilder.Entity<Attachment>().ToTable("Attachments");

            // Checkouts
            modelBuilder.Entity<Checkout>().ToTable("Checkouts");

            // Counters
            modelBuilder.Entity<Counter>().ToTable("Counters");

            // Companies
            modelBuilder.Entity<Company>().ToTable("Companies");

            // Employees
            modelBuilder.Entity<Employee>().ToTable("Employees");

            // Executions
            modelBuilder.Entity<Execution>().ToTable("Executions");

            // Resolutions
            modelBuilder.Entity<Resolution>().ToTable("Resolutions");

            // Incoming_docs
            modelBuilder.Entity<IncomingDocument>().ToTable("Incoming_docs");

            // Outgoing_docs
            modelBuilder.Entity<OutgoingDocument>().ToTable("Outgoing_docs");

            // Internal_docs
            modelBuilder.Entity<InternalDocument>().ToTable("Internal_docs");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=" + FileName);
        }
    }
}
