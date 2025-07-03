using ArticleDashboard.Application.Features.Articles.Commands.CreateArticle;
using ArticleDashboard.Application.Features.Articles.Commands.UpdateArticle;
using ArticleDashboard.Domain.Entities;
using ArticleDashboard.Domain.Enums;
using ArticleDashboard.Domain.Interfaces;
using AutoMapper;
using Moq;

namespace ArticleDashboard.Application.Tests.Features.Articles.Commands.UpdateArticle;

public class UpdateArticleHandlerTests
{
    private readonly Mock<IArticleRepository> _repositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly UpdateArticleHandler _handler;

    public UpdateArticleHandlerTests()
    {
        _handler = new UpdateArticleHandler(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateArticle_WhenEntityExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = new CreateArticleCommand
        {
            Name = "Updated Name",
            ArticleNumber = 200,
            Material = Material.Aluminum,
            ArticleCategory = ArticleCategory.Rim,
            NetWeight = 500,
            BicycleCategoryIds = [1, 3]
        };

        var existingArticle = new Article { Id = id, Name = "Old Name" };

        _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingArticle);
        _repositoryMock.Setup(r => r.UpdateAsync(existingArticle, command.BicycleCategoryIds)).Returns(Task.CompletedTask);

        // Act
        await _handler.Handle((id, command));

        // Assert
        _mapperMock.Verify(m => m.Map(command, existingArticle), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(existingArticle, command.BicycleCategoryIds), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenEntityNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = new CreateArticleCommand { Name = "Anything" };

        _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Article)null!);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => _handler.Handle((id, command)));
        Assert.Equal("Article not found", ex.Message);
    }
}
