using ArticleDashboard.Application.Common.Interfaces;
using ArticleDashboard.Application.DTOs;
using ArticleDashboard.Application.Features.Articles.Commands.CreateArticle;
using ArticleDashboard.Application.Features.Articles.Queries.GetAllArticles;
using ArticleDashboard.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ArticleDashboard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly ICommandHandler<CreateArticleCommand, Guid> _createHandler;
    private readonly ICommandHandler<(Guid, CreateArticleCommand)> _updateHandler;
    private readonly IQueryHandler<GetAllArticlesQuery, PagedResult<ArticleDto>> _getAllHandler;
    private readonly IQueryHandler<Guid, ArticleDto?> _getByIdHandler;

    public ArticlesController(
        ICommandHandler<CreateArticleCommand, Guid> createHandler,
        ICommandHandler<(Guid, CreateArticleCommand)> updateHandler,
        IQueryHandler<GetAllArticlesQuery, PagedResult<ArticleDto>> getAllHandler,
        IQueryHandler<Guid, ArticleDto?> getByIdHandler)
    {
        _createHandler = createHandler;
        _updateHandler = updateHandler;
        _getAllHandler = getAllHandler;
        _getByIdHandler = getByIdHandler;
    }

    /// <summary>
    /// Retrieves a paginated list of articles with optional filtering and sorting.
    /// </summary>
    /// <param name="articleCategory">Filter by article category (e.g., Hub, Crank).</param>
    /// <param name="bicycleCategoryIds">Filter by one or more bicycle category IDs.</param>
    /// <param name="material">Filter by material (e.g., Aluminum, Carbon).</param>
    /// <param name="sortBy">The field to sort by (e.g., netWeight, articleCategory).</param>
    /// <param name="sortDirection">The sort direction: 'asc' or 'desc'.</param>
    /// <param name="page">The page number to retrieve (starting from 1).</param>
    /// <param name="pageSize">The number of articles per page.</param>
    /// <returns>
    /// A paginated result containing a list of <see cref="ArticleDto"/> items.
    /// </returns>
    [HttpGet]
    public async Task<ActionResult<PagedResult<ArticleDto>>> Get(
        [FromQuery] ArticleCategory? articleCategory,
        [FromQuery] List<int>? bicycleCategoryIds,
        [FromQuery] Material? material,
        [FromQuery] string? sortBy,
        [FromQuery] string? sortDirection,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetAllArticlesQuery
        {
            ArticleCategory = articleCategory,
            BicycleCategoryIds = bicycleCategoryIds,
            Material = material,
            SortBy = sortBy,
            SortDirection = sortDirection,
            Page = page,
            PageSize = pageSize
        };

        var result = await _getAllHandler.Handle(query);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a single article by its unique identifier.
    /// </summary>
    /// <param name="id">The unique ID of the article to retrieve.</param>
    /// <returns>
    /// Returns the <see cref="ArticleDto"/> if found; otherwise, <c>404 Not Found</c>.
    /// </returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ArticleDto>> GetById(Guid id)
    {
        var article = await _getByIdHandler.Handle(id);
        if (article == null) return NotFound();
        return Ok(article);
    }

    /// <summary>
    /// Creates a new article.
    /// </summary>
    /// <param name="command">The article creation data.</param>
    /// <returns>
    /// Returns the ID of the newly created article and a <c>201 Created</c> response.
    /// </returns>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="201">If the article was successfully created.</response>
    [HttpPost]
    public async Task<ActionResult<Guid>> Post([FromBody] CreateArticleCommand command)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var id = await _createHandler.Handle(command);

        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    /// <summary>
    /// Updates an existing article.
    /// </summary>
    /// <param name="id">The ID of the article to update.</param>
    /// <param name="command">The updated article data.</param>
    /// <returns>
    /// Returns <c>204 No Content</c> if the update was successful.
    /// </returns>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="204">If the article was successfully updated.</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] CreateArticleCommand command)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        await _updateHandler.Handle((id, command));

        return NoContent();
    }
}
