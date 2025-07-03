using ArticleDashboard.Domain.Entities;
using ArticleDashboard.Domain.Enums;

namespace ArticleDashboard.Domain.Interfaces;

public interface IArticleRepository
{
    IQueryable<Article> GetFilteredQuery(
        ArticleCategory? category,
        List<int>? bicycleCategoryIds,
        Material? material,
        string? sortBy,
        string? sortDirection);
    Task<Article?> GetByIdAsync(Guid id);
    Task AddAsync(Article article, List<int>? bicycleCategoryIds);
    Task UpdateAsync(Article article, List<int>? bicycleCategoryIds);
}
