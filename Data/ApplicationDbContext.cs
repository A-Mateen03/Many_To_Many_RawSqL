using Many_To_Many_RawSqL.Models;
using Microsoft.EntityFrameworkCore;

namespace Many_To_Many_RawSqL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Buyer> buyers { get; set; }
        public DbSet<Products> Products { get; set; }

        public DbSet<BuyerProducts> BuyerProduct { get; set; }

    }
}
