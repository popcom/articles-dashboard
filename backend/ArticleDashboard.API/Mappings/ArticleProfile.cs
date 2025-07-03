using AutoMapper;
using ArticleDashboard.Application.DTOs;
using ArticleDashboard.Application.Features.Articles.Commands.CreateArticle;
using ArticleDashboard.Domain.Entities;

namespace ArticleDashboard.API.Mappings;

public class ArticleProfile : Profile
{
    public ArticleProfile()
    {
        CreateMap<Article, ArticleDto>().ReverseMap();
        CreateMap<CreateArticleCommand, Article>();
    }
}
