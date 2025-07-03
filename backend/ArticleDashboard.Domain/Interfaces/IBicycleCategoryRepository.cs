using ArticleDashboard.Domain.Entities;

namespace ArticleDashboard.Domain.Interfaces;

public interface IBicycleCategoryRepository
{
    //Task<List<BicycleCategory>> GetAll();
    IQueryable<BicycleCategory> GetAll();
    Task<BicycleCategory?> GetByIdAsync(int id);
    Task AddAsync(BicycleCategory bicycleCategory);
    Task UpdateAsync(BicycleCategory bicycleCategory);
}
