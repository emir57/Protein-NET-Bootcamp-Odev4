using Core.Utilities.Helpers;
using Microsoft.EntityFrameworkCore;
using Pagination.Entity.Concrete;

namespace Pagination.DataAccess.Contexts
{
    public class EfPaginationDbContext : DbContext
    {
        DbSet<Person> Persons { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .ToTable("Person");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionStringHelper.GetConnectionString("PostgreSqlConnection"));
        }
    }
}
