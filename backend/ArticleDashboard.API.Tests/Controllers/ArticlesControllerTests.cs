using ArticleDashboard.API.Controllers;
using ArticleDashboard.Application.Common.Interfaces;
using ArticleDashboard.Application.DTOs;
using ArticleDashboard.Application.Features.Articles.Commands.CreateArticle;
using ArticleDashboard.Application.Features.Articles.Queries.GetAllArticles;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ArticleDashboard.API.Tests.Controllers;

public class ArticlesControllerTests
{
    private readonly Mock<ICommandHandler<CreateArticleCommand, Guid>> _createHandlerMock = new();
    private readonly Mock<ICommandHandler<(Guid, CreateArticleCommand)>> _updateHandlerMock = new();
    private readonly Mock<IQueryHandler<GetAllArticlesQuery, PagedResult<ArticleDto>>> _getAllHandlerMock = new();
    private readonly Mock<IQueryHandler<Guid, ArticleDto?>> _getByIdHandlerMock = new();

    private readonly ArticlesController _controller;

    public ArticlesControllerTests()
    {
        _controller = new ArticlesController(
            _createHandlerMock.Object,
            _updateHandlerMock.Object,
            _getAllHandlerMock.Object,
            _getByIdHandlerMock.Object
        );
    }

    [Fact]
    public async Task Get_ReturnsPagedArticles()
    {
        // Arrange
        var result = new PagedResult<ArticleDto>
        {
            Items = new List<ArticleDto> { new ArticleDto { Name = "Test" } },
            TotalCount = 1
        };

        _getAllHandlerMock.Setup(x => x.Handle(It.IsAny<GetAllArticlesQuery>()))
            .ReturnsAsync(result);

        // Act
        var response = await _controller.Get(null, null, null, null, null, 1, 10);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(response.Result);
        var value = Assert.IsType<PagedResult<ArticleDto>>(okResult.Value);
        Assert.Equal(1, value.TotalCount);
        Assert.Single(value.Items);
    }


    [Fact]
    public async Task GetById_ReturnsArticle_WhenFound()
    {
        // Arrange
        var article = new ArticleDto { Id = Guid.NewGuid(), Name = "Test Article" };
        _getByIdHandlerMock.Setup(x => x.Handle(article.Id)).ReturnsAsync(article);

        // Act
        var result = await _controller.GetById(article.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var dto = Assert.IsType<ArticleDto>(okResult.Value);
        Assert.Equal("Test Article", dto.Name);
    }

    [Fact]
    public async Task Post_ReturnsCreatedId()
    {
        // Arrange
        var cmd = new CreateArticleCommand { Name = "New" };
        var newId = Guid.NewGuid();
        _createHandlerMock.Setup(x => x.Handle(cmd)).ReturnsAsync(newId);

        // Act
        var result = await _controller.Post(cmd);

        // Assert
        var createdAt = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(newId, createdAt.RouteValues["id"]);
    }

    [Fact]
    public async Task Put_ReturnsNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var cmd = new CreateArticleCommand { Name = "Updated" };

        _updateHandlerMock.Setup(x => x.Handle(It.IsAny<(Guid, CreateArticleCommand)>())).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Put(id, cmd);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
