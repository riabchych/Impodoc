using Microsoft.EntityFrameworkCore;

namespace ImpoDoc
{
    class ApplicationContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=core.db");
        }
    }
}
