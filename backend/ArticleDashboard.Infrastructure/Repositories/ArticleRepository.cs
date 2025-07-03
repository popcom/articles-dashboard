using ArticleDashboard.Domain.Entities;
using ArticleDashboard.Domain.Enums;
using ArticleDashboard.Domain.Interfaces;
using ArticleDashboard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArticleDashboard.Infrastructure.Repositories;

public class ArticleRepository : IArticleRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IBicycleCategoryRepository _bicycleCategoryRepository;

    public ArticleRepository(ApplicationDbContext context, IBicycleCategoryRepository bicycleCategoryRepository)
    {
        _context = context;
        _bicycleCategoryRepository = bicycleCategoryRepository;
    }

    public IQueryable<Article> GetFilteredQuery(
     ArticleCategory? cat,
     List<int>? bicycleCategoryIds,
     Material? mat,
     string? sortBy,
     string? sortDirection)
    {
        var query = _context.Articles.Include(q => q.BicycleCategories).AsQueryable().AsNoTracking();

        if (cat.HasValue)
            query = query.Where(x => x.ArticleCategory == cat);

        if (bicycleCategoryIds?.Count > 0)
            query = query.Where(x => x.BicycleCategories.Any(b => bicycleCategoryIds.Contains(b.Id)));

        if (mat.HasValue)
            query = query.Where(x => x.Material == mat);

        return (sortBy?.ToLower(), sortDirection?.ToLower()) switch
        {
            ("netweight", "asc") => query.OrderBy(x => x.NetWeight),
            ("netweight", "desc") => query.OrderByDescending(x => x.NetWeight),
            ("articlecategory", "asc") => query.OrderBy(x => x.ArticleCategory),
            ("articlecategory", "desc") => query.OrderByDescending(x => x.ArticleCategory),
            _ => query
        };
    }

    public Task<Article?> GetByIdAsync(Guid id) => _context.Articles.Include(p => p.BicycleCategories).FirstOrDefaultAsync(a => a.Id == id);

    public async Task AddAsync(Article article, List<int>? bicycleCategoryIds) {

        var bicycleCategories = _bicycleCategoryRepository.GetAll();
        var filteredBicycleCategories = bicycleCategories
        .Where(c => bicycleCategoryIds!.Contains(c.Id)).ToList();

        // Ensure EF does NOT try to insert them
        foreach (var category in filteredBicycleCategories)
        {
            _context.Entry(category).State = EntityState.Unchanged; 
        }
        article.BicycleCategories = filteredBicycleCategories;

        _context.Add(article); 
        await _context.SaveChangesAsync(); 
    }

    public async Task UpdateAsync(Article article, List<int>? bicycleCategoryIds) {

        var currentBicycleCategoryIds = article.BicycleCategories.Select(p => p.Id).ToList();
        var bicycleCategories = _bicycleCategoryRepository.GetAll();
        var filteredBicycleCategories = bicycleCategories
            .Where(c => bicycleCategoryIds!.Contains(c.Id)).ToList();

        // Ensure EF does NOT try to insert them
        foreach (var category in filteredBicycleCategories)
        {
            _context.Entry(category).State = EntityState.Unchanged;
        }
        article.BicycleCategories = filteredBicycleCategories;

        _context.Update(article); 
        await _context.SaveChangesAsync();
    }
}
