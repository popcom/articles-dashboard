using ArticleDashboard.Domain.Entities;
using ArticleDashboard.Domain.Enums;

namespace ArticleDashboard.Application.DTOs;

/// <summary>
/// Represents an article with its technical specifications and category assignments.
/// </summary>
public class ArticleDto
{
    /// <summary>
    /// The unique identifier of the article.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The internal reference number of the article (auto-incremented or generated).
    /// </summary>
    public int ArticleNumber { get; set; }

    /// <summary>
    /// The name of the article (e.g., \"Aluminum Crankset\").
    ///</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The type of article (e.g., Hub, Crank, Rim).
    /// </summary>
    public ArticleCategory ArticleCategory { get; set; }

    /// <summary>
    /// The list of bicycle categories that this article is compatible with.
    /// </summary>
    public List<BicycleCategory> BicycleCategories { get; set; } = [];

    /// <summary>
    /// The material used in the article (e.g., Aluminum, Carbon, Steel).
    /// </summary>
    public Material Material { get; set; }

    /// <summary>
    /// The net weight of the article in grams.
    /// </summary>
    public float NetWeight { get; set; }
}
