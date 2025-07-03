using ArticleDashboard.Application.Features.Articles.Commands;
using ArticleDashboard.Application.Features.Articles.Commands.CreateArticle;
using ArticleDashboard.Domain.Entities;
using ArticleDashboard.Domain.Enums;
using ArticleDashboard.Domain.Interfaces;
using AutoMapper;
using Moq;

namespace ArticleDashboard.Application.Tests.Features.Articles.Commands.CreateArticle;

public class CreateArticleHandlerTests
{
    private readonly Mock<IArticleRepository> _repositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    private readonly CreateArticleHandler _handler;

    public CreateArticleHandlerTests()
    {
        _handler = new CreateArticleHandler(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldMapAndSaveArticle_ReturnsId()
    {
        // Arrange
        var command = new CreateArticleCommand
        {
            Name = "Test Article",
            ArticleNumber = 123,
            NetWeight = 500,
            Material = Material.Aluminum,
            ArticleCategory = ArticleCategory.Rim,
            BicycleCategoryIds = [1, 2]
        };

        var article = new Article { Id = Guid.NewGuid(), Name = "Test Article" };

        _mapperMock.Setup(m => m.Map<Article>(command)).Returns(article);
        _repositoryMock.Setup(r => r.AddAsync(article, command.BicycleCategoryIds))
                       .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command);

        // Assert
        Assert.Equal(article.Id, result);
        _repositoryMock.Verify(r => r.AddAsync(article, command.BicycleCategoryIds), Times.Once);
    }
}
