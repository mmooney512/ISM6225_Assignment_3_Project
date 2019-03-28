using Microsoft.EntityFrameworkCore;
using ISM6225_Assignment_3_Project.Models;

namespace ISM6225_Assignment_3_Project.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<iex_api_Company> db_companies { get; set; }
        public DbSet<iex_api_pricing> db_prices { get; set; }
    }
}