using ArticleDashboard.Application.Common.Interfaces;
using ArticleDashboard.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BicycleCategoriesController : ControllerBase
{
    private readonly IQueryHandler<IEnumerable<BicycleCategory>> _getAllHandler;

    public BicycleCategoriesController(IQueryHandler<IEnumerable<BicycleCategory>> getAllHandler)
    {
        _getAllHandler = getAllHandler;
    }

    /// <summary>
    /// Retrieves a list of all available bicycle categories, ordered by name.
    /// </summary>
    /// <returns>
    /// A list of <see cref="BicycleCategory"/> objects.
    /// </returns>
    /// <response code="200">Returns the list of bicycle categories.</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BicycleCategory>>> Get()
    {
        var result = await _getAllHandler.Handle();
        return Ok(result);
    }
}
