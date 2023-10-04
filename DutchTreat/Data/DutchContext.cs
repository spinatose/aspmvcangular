using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DutchTreat.Data
{
    public class DutchContext : DbContext
    {
        private readonly IConfiguration _config;

        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        public DutchContext(IConfiguration config)
        {
            this._config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(this._config.GetConnectionString("DutchContextDb"));
        }

    }
}
