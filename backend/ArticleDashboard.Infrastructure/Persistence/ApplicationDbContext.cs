using ArticleDashboard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace ArticleDashboard.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<BicycleCategory> BicycleCategories => Set<BicycleCategory>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var listConverter = new ValueConverter<List<BicycleCategory>, string>(
    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
    v => JsonSerializer.Deserialize<List<BicycleCategory>>(v, (JsonSerializerOptions?)null)!);

        var listComparer = new ValueComparer<List<BicycleCategory>>(
            (c1, c2) => c1!.SequenceEqual(c2!),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList()
        );

        modelBuilder.Entity<Article>()
        .HasMany(a => a.BicycleCategories)
        .WithMany(b => b.Articles)
        .UsingEntity<Dictionary<string, object>>(
            "ArticleBicycleCategory",
            j => j.HasOne<BicycleCategory>().WithMany().HasForeignKey("BicycleCategoriesId"),
            j => j.HasOne<Article>().WithMany().HasForeignKey("ArticlesId"),
            j =>
            {
                j.HasKey("ArticlesId", "BicycleCategoriesId");
                j.ToTable("ArticleBicycleCategory");
            });

        modelBuilder.Entity<Article>()
            .Property(a => a.ArticleCategory)
            .HasConversion<string>();

        modelBuilder.Entity<Article>()
            .Property(a => a.Material)
            .HasConversion<string>();
    }
}
