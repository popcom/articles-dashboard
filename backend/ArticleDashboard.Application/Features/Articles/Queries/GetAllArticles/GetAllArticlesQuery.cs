using ArticleDashboard.Domain.Enums;

namespace ArticleDashboard.Application.Features.Articles.Queries.GetAllArticles;

public class GetAllArticlesQuery
{
    public ArticleCategory? ArticleCategory { get; set; }
    public List<int>? BicycleCategoryIds { get; set; }
    public Material? Material { get; set; }
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
    // Pagination
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
