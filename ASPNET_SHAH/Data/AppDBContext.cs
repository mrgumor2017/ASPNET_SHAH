using ASPNET_SHAH.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNET_SHAH.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options):
            base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
    
}
