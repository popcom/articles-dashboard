using ArticleDashboard.Application.Common.Interfaces;
using ArticleDashboard.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ArticleDashboard.API.Tests.Controllers;

public class BicycleCategoriesControllerTests
{
    private readonly Mock<IQueryHandler<IEnumerable<BicycleCategory>>> _getAllHandlerMock = new();
    private readonly BicycleCategoriesController _controller;

    public BicycleCategoriesControllerTests()
    {
        _controller = new BicycleCategoriesController(_getAllHandlerMock.Object);
    }

    [Fact]
    public async Task Get_ReturnsOkWithCategories()
    {
        // Arrange
        var mockCategories = new List<BicycleCategory>
        {
            new BicycleCategory { Id = 1, Name = "Road" },
            new BicycleCategory { Id = 2, Name = "Mountain" }
        };

        _getAllHandlerMock.Setup(x => x.Handle())
                          .ReturnsAsync(mockCategories);

        // Act
        var result = await _controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsAssignableFrom<IEnumerable<BicycleCategory>>(okResult.Value);
        Assert.Equal(2, value.Count());
    }
}
