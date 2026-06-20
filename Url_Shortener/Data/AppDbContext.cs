using Microsoft.EntityFrameworkCore;
using Url_Shortener.Models;

namespace Url_Shortener.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ShortUrl> ShortUrls => Set<ShortUrl>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShortUrl>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OriginalUrl).IsRequired().HasMaxLength(2048);
                entity.Property(e => e.ShortCode).HasMaxLength(20);
                entity.HasIndex(e => e.ShortCode).IsUnique();
            });
        }
    }
}
