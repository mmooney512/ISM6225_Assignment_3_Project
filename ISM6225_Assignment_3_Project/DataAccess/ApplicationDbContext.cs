using Microsoft.EntityFrameworkCore;
using ISM6225_Assignment_3_Project.Models;

namespace ISM6225_Assignment_3_Project.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<iex_api_Company> db_companies { get; set; }
        public DbSet<iex_api_pricing> db_prices { get; set; }
        public DbSet<fx_api_Rates> db_fx { get; set; }
        public DbQuery<iex_fx_chart_Stock_Prices> view_fx_pricing { get; set; }
        public DbQuery<fx_api_fx_rates> view_fx_rates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<iex_api_pricing>().HasKey(ck => new { ck.symbol, ck.date });
        }
    }
}