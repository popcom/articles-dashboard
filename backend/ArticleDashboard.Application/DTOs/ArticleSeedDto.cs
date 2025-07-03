using ArticleDashboard.Domain.Enums;

namespace ArticleDashboard.Application.DTOs;

public class ArticleSeedDto
{
    public int ArticleNumber { get; set; }
    public string Name { get; set; } = string.Empty;
    public ArticleCategory ArticleCategory { get; set; }
    public Material Material { get; set; }
    public float NetWeight { get; set; }
    public List<int> BicycleCategoryIds { get; set; } = [];
}
