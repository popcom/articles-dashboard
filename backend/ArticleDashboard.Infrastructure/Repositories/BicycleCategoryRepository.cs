using ArticleDashboard.Domain.Entities;
using ArticleDashboard.Domain.Interfaces;
using ArticleDashboard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArticleDashboard.Infrastructure.Repositories;

public class BicycleCategoryRepository : IBicycleCategoryRepository
{
    private readonly ApplicationDbContext _context;

    public BicycleCategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<BicycleCategory> GetAll()
    {
        return _context.BicycleCategories;
    }

    public Task<BicycleCategory?> GetByIdAsync(int id) => _context.BicycleCategories
        .FirstOrDefaultAsync(bc => bc.Id == id);

    public async Task AddAsync(BicycleCategory bicycleCategory)
    {
        _context.Add(bicycleCategory);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(BicycleCategory bicycleCategory)
    {
        _context.Update(bicycleCategory);
        await _context.SaveChangesAsync();
    }
}
