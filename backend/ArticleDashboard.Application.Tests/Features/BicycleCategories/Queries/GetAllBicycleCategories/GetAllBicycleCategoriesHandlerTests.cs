using ArticleDashboard.Application.Features.BicycleCategories.Queries.GetAllBicycleCategories;
using ArticleDashboard.Domain.Entities;
using ArticleDashboard.Domain.Interfaces;
using MockQueryable;
using Moq;

namespace ArticleDashboard.Application.Tests.Features.BicycleCategories.Queries.GetAllBicycleCategories;

public class GetAllBicycleCategoriesHandlerTests
{
    private readonly Mock<IBicycleCategoryRepository> _repoMock = new();
    private readonly GetAllBicycleCategoriesHandler _handler;

    public GetAllBicycleCategoriesHandlerTests()
    {
        _handler = new GetAllBicycleCategoriesHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsAllCategories()
    {
        // Arrange
        var data = new List<BicycleCategory>
        {
            new() { Id = 1, Name = "Road" },
            new() { Id = 2, Name = "Mountain" }
        };

        var mockQueryable = data.AsQueryable().BuildMock();
        _repoMock.Setup(r => r.GetAll()).Returns(mockQueryable);

        // Act
        var result = await _handler.Handle();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, c => c.Name == "Road");
    }
}
