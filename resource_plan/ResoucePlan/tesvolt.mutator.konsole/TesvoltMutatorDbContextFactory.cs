using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tesvolt.mutator.console
{
    public class TesvoltMutatorDbContextFactory : IDesignTimeDbContextFactory<TesvoltMutatorDbContext>
    {
        public TesvoltMutatorDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TesvoltMutatorDbContext>();
            var connectionString = configuration.GetConnectionString("TesvoltDbContext");

            optionsBuilder.UseSqlServer(connectionString);

            return new TesvoltMutatorDbContext(optionsBuilder.Options);
        }
    }
}
