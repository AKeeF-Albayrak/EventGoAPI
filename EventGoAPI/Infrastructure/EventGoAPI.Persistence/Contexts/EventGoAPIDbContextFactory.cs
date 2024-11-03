using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EventGoAPI.Persistence.Context
{
    public class EventGoDbContextFactory : IDesignTimeDbContextFactory<EventGoDbContext>
    {
        public EventGoDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<EventGoDbContextFactory>()
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<EventGoDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new EventGoDbContext(optionsBuilder.Options);
        }
    }
}