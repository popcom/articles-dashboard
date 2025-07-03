using ArticleDashboard.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ArticleDashboard.Application.Features.Articles.Commands.CreateArticle;

public class CreateArticleCommand
{
    [Required(ErrorMessage = "Article Number is required")]
    public int ArticleNumber { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EnumDataType(typeof(ArticleCategory))]
    public ArticleCategory ArticleCategory { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one bicycle category is required.")]
    public List<int> BicycleCategoryIds { get; set; } = [];

    [Required]
    [EnumDataType(typeof(Material))]
    public Material Material { get; set; }

    [Range(0.1, double.MaxValue, ErrorMessage = "Net weight must be greater than 0.")]
    public float NetWeight { get; set; }
}
