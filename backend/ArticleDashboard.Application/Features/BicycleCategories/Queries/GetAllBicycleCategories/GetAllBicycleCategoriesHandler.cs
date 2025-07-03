using ArticleDashboard.Application.Common.Interfaces;
using ArticleDashboard.Domain.Entities;
using ArticleDashboard.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArticleDashboard.Application.Features.BicycleCategories.Queries.GetAllBicycleCategories;

public class GetAllBicycleCategoriesHandler : IQueryHandler<IEnumerable<BicycleCategory>>
{
    private readonly IBicycleCategoryRepository _repo;

    public GetAllBicycleCategoriesHandler(IBicycleCategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<BicycleCategory>> Handle()
    {
        return await _repo.GetAll().ToListAsync();
    }
}
