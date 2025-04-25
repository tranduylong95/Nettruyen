using Microsoft.EntityFrameworkCore;
using nettruyen.Model;
namespace nettruyen.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Category> Categories { get; set; }
    }
}
