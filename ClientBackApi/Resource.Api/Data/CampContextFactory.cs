using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Resource.Api.Data
{
    public class CampContextFactory : IDesignTimeDbContextFactory<CampContext>
    {
        public CampContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
              .Build();

            return new CampContext(new DbContextOptionsBuilder<CampContext>().Options, config);
        }
    }
}
