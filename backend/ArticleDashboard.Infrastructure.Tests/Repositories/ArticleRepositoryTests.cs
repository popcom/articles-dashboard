using ArticleDashboard.Domain.Entities;
using ArticleDashboard.Domain.Enums;
using ArticleDashboard.Infrastructure.Persistence;
using ArticleDashboard.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArticleDashboard.Infrastructure.Tests.Repositories;

public class ArticleRepositoryTests
{
    private readonly ApplicationDbContext _context;
    private readonly ArticleRepository _repository;
    private readonly BicycleCategoryRepository _bicycleRepo;

    public ArticleRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _bicycleRepo = new BicycleCategoryRepository(_context);
        _repository = new ArticleRepository(_context, _bicycleRepo);

        SeedData();
    }

    private void SeedData()
    {
        _context.BicycleCategories.RemoveRange(_context.BicycleCategories);
        _context.Articles.RemoveRange(_context.Articles);
        _context.SaveChanges();

        var road = new BicycleCategory { Id = 1, Name = "Road" };
        var mountain = new BicycleCategory { Id = 2, Name = "Mountain" };

        var a1 = new Article
        {
            Id = Guid.NewGuid(),
            Name = "Crank A",
            ArticleNumber = 100,
            ArticleCategory = ArticleCategory.Crank,
            Material = Material.Aluminum,
            NetWeight = 600,
            BicycleCategories = [road]
        };

        var a2 = new Article
        {
            Id = Guid.NewGuid(),
            Name = "Hub B",
            ArticleNumber = 101,
            ArticleCategory = ArticleCategory.Hub,
            Material = Material.Steel,
            NetWeight = 700,
            BicycleCategories = [mountain]
        };

        _context.BicycleCategories.AddRange(road, mountain);
        _context.Articles.AddRange(a1, a2);
        _context.SaveChanges();
    }

    [Fact]
    public void GetFilteredQuery_ByArticleCategory_ShouldReturnMatching()
    {
        var result = _repository
            .GetFilteredQuery(ArticleCategory.Crank, null, null, null, null)
            .ToList();

        Assert.Single(result);
        Assert.Equal(ArticleCategory.Crank, result[0].ArticleCategory);
    }

    [Fact]
    public void GetFilteredQuery_ByMaterial_ShouldReturnMatching()
    {
        var result = _repository
            .GetFilteredQuery(null, null, Material.Steel, null, null)
            .ToList();

        Assert.Single(result);
        Assert.Equal(Material.Steel, result[0].Material);
    }

    [Fact]
    public void GetFilteredQuery_ByBicycleCategoryIds_ShouldReturnMatching()
    {
        var result = _repository
            .GetFilteredQuery(null, new List<int> { 1 }, null, null, null)
            .ToList();

        Assert.Single(result);
        Assert.Contains("Crank", result[0].Name);
    }

    [Fact]
    public void GetFilteredQuery_SortingByNetWeightDesc_ShouldWork()
    {
        var result = _repository
            .GetFilteredQuery(null, null, null, "netWeight", "desc")
            .ToList();

        Assert.Equal(700, result[0].NetWeight);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnArticleWithCategories()
    {
        // Arrange
        var expected = _context.Articles.Include(a => a.BicycleCategories).First();

        // Act
        var result = await _repository.GetByIdAsync(expected.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.Id, result!.Id);
        Assert.NotEmpty(result.BicycleCategories);
    }


    [Fact]
    public async Task AddAsync_ShouldInsertNewArticleWithRelations()
    {
        var article = new Article
        {
            Id = Guid.NewGuid(),
            Name = "New Rim",
            ArticleNumber = 300,
            ArticleCategory = ArticleCategory.Rim,
            Material = Material.Carbon,
            NetWeight = 420
        };

        await _repository.AddAsync(article, new List<int> { 1, 2 });

        var saved = await _context.Articles
            .Include(a => a.BicycleCategories)
            .FirstOrDefaultAsync(a => a.Id == article.Id);

        Assert.NotNull(saved);
        Assert.Equal("New Rim", saved!.Name);
        Assert.Equal(2, saved.BicycleCategories.Count);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateArticleAndCategories()
    {
        var existing = _context.Articles.Include(a => a.BicycleCategories).First();
        var newCategories = new List<int> { 2 }; // change to only mountain

        existing.Name = "Updated Crank";
        existing.NetWeight = 555;

        await _repository.UpdateAsync(existing, newCategories);

        var updated = await _context.Articles
            .Include(a => a.BicycleCategories)
            .FirstOrDefaultAsync(a => a.Id == existing.Id);

        Assert.NotNull(updated);
        Assert.Equal("Updated Crank", updated!.Name);
        Assert.Equal(555, updated.NetWeight);
        Assert.Single(updated.BicycleCategories);
        Assert.Equal("Mountain", updated.BicycleCategories.First().Name);
    }
}
