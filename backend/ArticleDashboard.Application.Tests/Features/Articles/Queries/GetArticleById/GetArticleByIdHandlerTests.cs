using ArticleDashboard.Application.DTOs;
using ArticleDashboard.Application.Features.Articles.Queries.GetArticleById;
using ArticleDashboard.Domain.Entities;
using ArticleDashboard.Domain.Interfaces;
using AutoMapper;
using Moq;

namespace ArticleDashboard.Application.Tests.Features.Articles.Queries.GetArticleById;

public class GetArticleByIdHandlerTests
{
    private readonly Mock<IArticleRepository> _repoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly GetArticleByIdHandler _handler;

    public GetArticleByIdHandlerTests()
    {
        _handler = new GetArticleByIdHandler(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsMappedArticleDto_WhenArticleExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var article = new Article { Id = id, Name = "Test Article" };
        var dto = new ArticleDto { Id = id, Name = "Test Article" };

        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(article);
        _mapperMock.Setup(m => m.Map<ArticleDto>(article)).Returns(dto);

        // Act
        var result = await _handler.Handle(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Article", result?.Name);
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenArticleDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Article)null!);

        // Act
        var result = await _handler.Handle(id);

        // Assert
        Assert.Null(result);
        _mapperMock.Verify(m => m.Map<ArticleDto>(It.IsAny<Article>()), Times.Never);
    }
}
