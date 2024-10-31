using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public class BClouderDbContext : DbContext
    {
        private IConfiguration _config;

        public BClouderDbContext(IConfiguration config, DbContextOptions options): base(options)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public DbSet<BClouderTask> Tasks { get; set; }
        public DbSet<BClouderUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BClouderTask>().HasQueryFilter(t => !t.IsDeleted);
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<BClouderUser>()
        //        .Ignore(u => u.Tasks); // Ignorar a propriedade Phones para evitar referencia circular.
        //}


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var typeDatabase = _config["TypeDatabase"];
        //    if (typeDatabase != null)
        //    {
        //        var connectionString = _config.GetConnectionString(typeDatabase);

        //        if (connectionString == "SqlServer")
        //        {
        //            optionsBuilder.UseSqlServer(connectionString);
        //        }
        //        //else if (connectionString == "Postgreesql")
        //        //{
        //        //    optionsBuilder.UseNpgsql(connectionString);
        //        //}
        //        //else if (connectionString == "SqlServer")
        //        //{
        //        //    optionsBuilder.UseMySQL(connectionString);
        //        //}
        //    }

        //}
    }
}
