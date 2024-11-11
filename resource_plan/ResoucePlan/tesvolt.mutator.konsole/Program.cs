// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using tesvolt.dbmodels.app;
using tesvolt.mutator.console;

//var services = new ServiceCollection();

//// Configure the DbContext with your connection string
////services.AddDbContext<TesvoltMutatorDbContext>(options =>
////   options.UseSqlServer("Server=TV-AMAT162404\\SQLEXPRESS;Database=Resouceplan_1;MultipleActiveResultSets=true;TrustServerCertificate=True;"));

////services.AddDbContext<TesvoltMutatorDbContext>(options =>
////   options.UseSqlServer("Server=DESKTOP-SJAS0BV\\SQLEXPRESS;Database=re_sorce;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"));
//services.AddDbContext<TesvoltDbContext>(options =>
//    options.UseSqlServer(services.Configuration.GetConnectionString("TesvoltDbContext")));

var services = new ServiceCollection();

// Build a configuration object to read from appsettings.json
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Use the connection string from appsettings.json to configure the DbContext
services.AddDbContext<TesvoltMutatorDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("TesvoltDbContext")));
Console.WriteLine("Hello, World!");
