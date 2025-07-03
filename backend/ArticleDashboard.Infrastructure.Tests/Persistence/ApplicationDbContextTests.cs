using ArticleDashboard.Domain.Entities;
using ArticleDashboard.Domain.Enums;
using ArticleDashboard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArticleDashboard.Infrastructure.Tests.Persistence;

public class ApplicationDbContextTests
{
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);

        SeedData();
    }

    private void SeedData()
    {
        _context.BicycleCategories.RemoveRange(_context.BicycleCategories);
        _context.Articles.RemoveRange(_context.Articles);
        _context.SaveChanges();

        _context.BicycleCategories.AddRange(
            new BicycleCategory { Id = 1, Name = "Road" },
            new BicycleCategory { Id = 2, Name = "Mountain" }
        );
        _context.SaveChanges();
    }

    [Fact]
    public void CanInsertBicycleCategory()
    {
        var gravel = new BicycleCategory { Name = "Gravel" };
        _context.BicycleCategories.Add(gravel);
        _context.SaveChanges();

        Assert.True(gravel.Id > 0);
    }

    [Fact]
    public void CanInsertArticleWithCategories()
    {
        var road = _context.BicycleCategories.First(c => c.Name == "Road");
        var mountain = _context.BicycleCategories.First(c => c.Name == "Mountain");

        var article = new Article
        {
            Id = Guid.NewGuid(),
            ArticleNumber = 101,
            Name = "Test Crank",
            ArticleCategory = ArticleCategory.Crank,
            Material = Material.Steel,
            NetWeight = 420,
            BicycleCategories = [road, mountain]
        };

        _context.Articles.Add(article);
        _context.SaveChanges();

        var stored = _context.Articles
            .Include(a => a.BicycleCategories)
            .FirstOrDefault(a => a.Id == article.Id);

        Assert.NotNull(stored);
        Assert.Equal(2, stored!.BicycleCategories.Count);
    }

    [Fact]
    public void HasDbSetsConfigured()
    {
        Assert.NotNull(_context.Articles);
        Assert.NotNull(_context.BicycleCategories);
    }
}
