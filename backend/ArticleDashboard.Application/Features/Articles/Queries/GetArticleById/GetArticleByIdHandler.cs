using ArticleDashboard.Application.Common.Interfaces;
using ArticleDashboard.Application.DTOs;
using ArticleDashboard.Domain.Interfaces;
using AutoMapper;

namespace ArticleDashboard.Application.Features.Articles.Queries.GetArticleById;

public class GetArticleByIdHandler : IQueryHandler<Guid, ArticleDto?>
{
    private readonly IArticleRepository _repo;
    private readonly IMapper _mapper;

    public GetArticleByIdHandler(IArticleRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ArticleDto?> Handle(Guid id)
    {
        var article = await _repo.GetByIdAsync(id);
        return article is null ? null : _mapper.Map<ArticleDto>(article);
    }
}