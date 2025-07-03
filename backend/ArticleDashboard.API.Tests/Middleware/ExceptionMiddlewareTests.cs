using ArticleDashboard.API.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Text.Json;

namespace ArticleDashboard.API.Tests.Middleware;

public class ExceptionMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_WhenExceptionThrown_Returns500AndLogsError()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ExceptionMiddleware>>();
        var context = new DefaultHttpContext();
        var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        RequestDelegate next = (_) => throw new InvalidOperationException("Something went wrong");

        var middleware = new ExceptionMiddleware(next, loggerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var json = await new StreamReader(context.Response.Body).ReadToEndAsync();

        Assert.False(string.IsNullOrWhiteSpace(json));

        var parsed = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        Assert.NotNull(parsed);
        Assert.True(parsed!.ContainsKey("message"), "Response JSON must contain 'message'");
        Assert.Equal("Something went wrong", parsed["message"]);

        loggerMock.Verify(
            l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((_, __) => true),  // accept any log state
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
