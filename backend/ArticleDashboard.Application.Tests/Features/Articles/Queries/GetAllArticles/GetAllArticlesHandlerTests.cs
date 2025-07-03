using ArticleDashboard.Application.DTOs;
using ArticleDashboard.Application.Features.Articles.Queries.GetAllArticles;
using ArticleDashboard.Domain.Entities;
using ArticleDashboard.Domain.Enums;
using ArticleDashboard.Domain.Interfaces;
using AutoMapper;
using MockQueryable;
using Moq;

namespace ArticleDashboard.Application.Tests.Features.Articles.Queries.GetAllArticles;

public class GetAllArticlesHandlerTests
{
    private readonly Mock<IArticleRepository> _repoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly GetAllArticlesHandler _handler;

    public GetAllArticlesHandlerTests()
    {
        _handler = new GetAllArticlesHandler(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPagedArticles()
    {
        // Arrange
        var articles = new List<Article>
        {
            new() { Name = "A1" },
            new() { Name = "A2" },
            new() { Name = "A3" }
        };

        // For using Moq.EntityFrameworkCore or similar
        var queryable = articles.AsQueryable().BuildMock(); 

        _repoMock.Setup(r => r.GetFilteredQuery(It.IsAny<ArticleCategory?>(), It.IsAny<List<int>?>(), It.IsAny<Material?>(), It.IsAny<string?>(), It.IsAny<string?>()))
                 .Returns(queryable);

        _mapperMock.Setup(m => m.Map<List<ArticleDto>>(It.IsAny<List<Article>>()))
                   .Returns((List<Article> a) => a.Select(x => new ArticleDto { Name = x.Name }).ToList());

        var query = new GetAllArticlesQuery
        {
            Page = 1,
            PageSize = 2,
            SortBy = "name"
        };

        // Act
        var result = await _handler.Handle(query);

        // Assert
        Assert.Equal(3, result.TotalCount);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal("A1", result.Items[0].Name);
    }
}
