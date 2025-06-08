using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<Subscription> Subscriptions => Set<Subscription>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.ToTable("Subscriptions");

            entity.Property(s => s.CreatedDate).HasColumnType("TEXT");
            entity.Property(s => s.CanceledDate).HasColumnType("TEXT");

            entity.Property(s => s.SubscriptionInterval).IsRequired();
        });
    }
}