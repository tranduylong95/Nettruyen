using Microsoft.EntityFrameworkCore;
using nettruyen.Model;
namespace nettruyen.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comic> Comic { get; set; }
        public DbSet<ComicCategory> ComicCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ComicCategory>()
            .ToTable("comic_category");

            modelBuilder.Entity<ComicCategory>()
            .HasKey(cc => new { cc.idComic, cc.idCategory });
           
            modelBuilder.Entity<ComicCategory>()
            .HasOne(cc => cc.Comic)
            .WithMany(c => c.ComicCategories)
            .HasForeignKey(cc => cc.idComic)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ComicCategory>()
                .HasOne(cc => cc.Category)
                .WithMany(c => c.ComicCategories)
                .HasForeignKey(cc => cc.idCategory)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
