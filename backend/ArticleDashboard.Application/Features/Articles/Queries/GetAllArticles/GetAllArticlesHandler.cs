using ArticleDashboard.Application.Common.Interfaces;
using ArticleDashboard.Application.DTOs;
using ArticleDashboard.Domain.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ArticleDashboard.Application.Features.Articles.Queries.GetAllArticles;

public class GetAllArticlesHandler : IQueryHandler<GetAllArticlesQuery, PagedResult<ArticleDto>>
{
    private readonly IArticleRepository _repo;
    private readonly IMapper _mapper;

    public GetAllArticlesHandler(
        IArticleRepository repo,
        IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<PagedResult<ArticleDto>> Handle(GetAllArticlesQuery query)
    {
        var queryable = _repo.GetFilteredQuery(
            query.ArticleCategory,
            query.BicycleCategoryIds,
            query.Material,
            query.SortBy,
            query.SortDirection);

        var totalCount = await queryable.CountAsync();

        var pagedItems = await queryable
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        var result = new PagedResult<ArticleDto>
        {
            Items = _mapper.Map<List<ArticleDto>>(pagedItems),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };

        return result;
    }
}
