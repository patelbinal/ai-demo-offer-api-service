using Microsoft.EntityFrameworkCore;

namespace OfferApiService.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Seller> Sellers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Offer>(entity =>
            {
                entity.HasOne(o => o.Seller)
                      .WithMany(s => s.Offers)
                      .HasForeignKey(o => o.SellerId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.OwnsOne(o => o.Location);
                entity.OwnsOne(o => o.Condition);
            });
        }
    }
}
