using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BlueWave.Data.Context
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseMySql(
                "Server=localhost;Database=bluewave;User=root;Password=;",
                new MySqlServerVersion(new Version(8, 0, 0))
            );

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}