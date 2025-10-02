using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BlogApp.Infrastructure.Persistence
{
    // Migration işlemleri için tasarım zamanı DbContext fabrikası
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlite("Data Source=BlogApp.db"); // İleride doğrudan appsettings.json'den okunabilir.
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
