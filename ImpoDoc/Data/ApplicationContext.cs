using Microsoft.EntityFrameworkCore;

namespace ImpoDoc.Data
{
    public class ApplicationContext<TEntity> : DbContext
        where TEntity : class, new()
    {
        protected virtual string Name { get; set; }
        protected virtual string FileName { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TEntity>().ToTable(Name);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=" + FileName);
        }
    }
}
