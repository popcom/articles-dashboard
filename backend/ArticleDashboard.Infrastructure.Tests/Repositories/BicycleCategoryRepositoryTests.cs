using ArticleDashboard.Domain.Entities;
using ArticleDashboard.Infrastructure.Persistence;
using ArticleDashboard.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArticleDashboard.Infrastructure.Tests.Repositories;

public class BicycleCategoryRepositoryTests
{
    private readonly ApplicationDbContext _context;
    private readonly BicycleCategoryRepository _repository;

    public BicycleCategoryRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new BicycleCategoryRepository(_context);

        SeedData();
    }

    private void SeedData()
    {
        _context.BicycleCategories.RemoveRange(_context.BicycleCategories);
        _context.SaveChanges();

        _context.BicycleCategories.Add(new BicycleCategory { Id = 1, Name = "Road" });
        _context.BicycleCategories.Add(new BicycleCategory { Id = 2, Name = "Mountain" });
        _context.SaveChanges();
    }

    [Fact]
    public void GetAll_ReturnsAllCategories()
    {
        var result = _repository.GetAll().AsNoTracking().ToList(); // safer with no tracking

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectCategory()
    {
        var result = await _repository.GetByIdAsync(2);

        Assert.NotNull(result);
        Assert.Equal("Mountain", result!.Name);
    }

    [Fact]
    public async Task AddAsync_ShouldInsertCategory()
    {
        var newCat = new BicycleCategory { Id = 3, Name = "Gravel" };

        await _repository.AddAsync(newCat);

        var found = await _repository.GetByIdAsync(3);
        Assert.NotNull(found);
        Assert.Equal("Gravel", found!.Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyCategory()
    {
        var existing = await _repository.GetByIdAsync(1);
        existing!.Name = "Updated Road";

        await _repository.UpdateAsync(existing);

        var updated = await _repository.GetByIdAsync(1);
        Assert.Equal("Updated Road", updated!.Name);
    }
}
