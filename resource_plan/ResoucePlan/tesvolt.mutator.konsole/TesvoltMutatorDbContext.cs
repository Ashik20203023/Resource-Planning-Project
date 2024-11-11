using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using tesvolt.dbmodels.app;

namespace tesvolt.mutator.console
{
    public class TesvoltMutatorDbContext : DbContext
    {
        
                   
            public TesvoltMutatorDbContext(DbContextOptions<TesvoltMutatorDbContext> options)
                : base(options)
            {
            }


        public DbSet<ResourceModel> ResourceModels { get; set; }
        public DbSet<ProjectModel> ProjectModels { get; set; }
        public DbSet<VacationPlanModel> VacationPlanModels { get; set; }
        public DbSet<AttendanceEntryModel> AttendanceEntryModels { get; set; }
        public DbSet<AttendanceDetailEntryModel> AttendanceDetailEntryModels { get; set; }
        public DbSet<ResourcePlanModel> ResourcePlanModels { get; set; }
        public DbSet<ModuleModel> ModuleModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResourceModel>()
                .HasMany(r => r.VacationPlans)
                .WithOne(v => v.ResourceModel)
                .HasForeignKey(v => v.ResourceModelId)
                .OnDelete(DeleteBehavior.Cascade); // or whatever behavior you want
        }



        // This enables cascade delete

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<ResourceModel>().ToTable("Contact");

        //}
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var builder = new ConfigurationBuilder().AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: true);
        //    IConfigurationRoot configuration = builder.Build();


        //    optionsBuilder.UseSqlServer(configuration.GetConnectionString("WebApiDatabase"));
        //    base.OnConfiguring(optionsBuilder);
        //}

        //public ILogger<TesvoltDbContext> Logger { get; protected set; }

        //public TesvoltDbContext() : base() { }

        //public TesvoltDbContext(DbContextOptions<DbContext> options, ILogger<TesvoltDbContext> logger) : base(options)
        //{
        //    Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        //}
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<TesvoltDbContext>().ToTable("Contact");

        //}
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var builder = new ConfigurationBuilder().AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: true);
        //    IConfigurationRoot configuration = builder.Build();


        //    optionsBuilder.UseSqfiguration.GetConnectionString("WebApiDatabase"));
        //    base.OnConfiguring(optionsBuilder);
        //}
        //public DbSet<ResourceModel> ResourceModels { get; set; }

        //public DbSet<Testy> Testies { get; set; }



        //protected readonly IConfiguration Configuration;

        //public MatelsoDataContext(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        //public MatelsoDataContext() : base()
        //{
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //{
        //    // connect to postgres with connection string from app settings
        //    options.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
        //}
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<Contact>().ToTable("Contact");

        //}

        //public DbSet<Contact> Contacts { get; set; }
    }

}
