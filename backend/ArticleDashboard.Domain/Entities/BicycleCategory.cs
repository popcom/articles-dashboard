using System.Text.Json.Serialization;

namespace ArticleDashboard.Domain.Entities;

/// <summary>
/// Represents a bicycle category, such as Road, Mountain, or Hybrid.
/// Used to group articles by compatible bicycle types.
/// </summary>
public class BicycleCategory
{
    /// /// <summary>
    /// The unique identifier of the bicycle category.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The display name of the bicycle category (e.g., Road, Mountain).
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The list of articles associated with this bicycle category.
    /// This property is ignored during JSON serialization.
    /// </summary>
    [JsonIgnore]
    public ICollection<Article> Articles { get; set; } = new List<Article>();
}