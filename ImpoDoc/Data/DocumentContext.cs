using ImpoDoc.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImpoDoc.Data
{
    public class DocumentContext : ApplicationContext<Document>
    {
        protected override string Name => "documents";
        protected override string FileName => "document.db";

        protected DbSet<IncomingDocument> IncomingDocs { get; set; }
        protected DbSet<OutgoingDocument> OutgoinglDocs { get; set; }
        protected DbSet<InternalDocument> InternalDocs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IncomingDocument>().ToTable("Incoming");
            modelBuilder.Entity<OutgoingDocument>().ToTable("Outgoing");
            modelBuilder.Entity<InternalDocument>().ToTable("Internal");
        }
    }
}
