using ArticleDashboard.Application.Common.Interfaces;
using ArticleDashboard.Application.Features.Articles.Commands.CreateArticle;
using ArticleDashboard.Domain.Interfaces;
using AutoMapper;

namespace ArticleDashboard.Application.Features.Articles.Commands.UpdateArticle;

public class UpdateArticleHandler : ICommandHandler<(Guid id, CreateArticleCommand command)>
{
    private readonly IArticleRepository _repo;
    private readonly IMapper _mapper;

    public UpdateArticleHandler(IArticleRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task Handle((Guid id, CreateArticleCommand command) input)
    {
        var (id, command) = input;
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) throw new Exception("Article not found");

        _mapper.Map(command, entity);
        await _repo.UpdateAsync(entity, command.BicycleCategoryIds);
    }
}