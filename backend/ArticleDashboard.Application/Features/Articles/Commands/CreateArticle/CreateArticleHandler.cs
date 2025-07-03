using ArticleDashboard.Application.Common.Interfaces;
using ArticleDashboard.Application.Features.Articles.Commands.CreateArticle;
using ArticleDashboard.Domain.Entities;
using ArticleDashboard.Domain.Interfaces;
using AutoMapper;

namespace ArticleDashboard.Application.Features.Articles.Commands;

public class CreateArticleHandler : ICommandHandler<CreateArticleCommand, Guid>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IMapper _mapper;


    public CreateArticleHandler(IArticleRepository articleRepository, IMapper mapper)
    {
        _articleRepository = articleRepository;
        _mapper = mapper;
    }


    public async Task<Guid> Handle(CreateArticleCommand cmd)
    {       
        var entity = _mapper.Map<Article>(cmd);
        await _articleRepository.AddAsync(entity, cmd.BicycleCategoryIds);
        return entity.Id;
    }
}