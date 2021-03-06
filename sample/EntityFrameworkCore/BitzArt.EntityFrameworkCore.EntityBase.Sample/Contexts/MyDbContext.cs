using BitzArt.EntityFrameworkCore.EntityBase.Sample.Models;
using Microsoft.EntityFrameworkCore;

namespace BitzArt.EntityFrameworkCore.EntityBase.Sample.Contexts
{
    public class MyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public MyDbContext(DbContextOptions options) : base(options) { }
    }
}
