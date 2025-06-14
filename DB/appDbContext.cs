using cat_API.Models;
using Microsoft.EntityFrameworkCore;

namespace cat_API.DB
{
    public class appDbContext : DbContext
    {
        public appDbContext(DbContextOptions<appDbContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<CatModel> Cats { get; set; }

    }
}
