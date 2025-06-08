using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Subscription> Subscriptions => Set<Subscription>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subscription>().ToTable("Subscriptions");

        modelBuilder.Entity<Subscription>()
            .Property(s => s.SubscriptionStartDate)
            .HasColumnType("TEXT");
    }
}