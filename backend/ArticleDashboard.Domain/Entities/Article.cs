using ArticleDashboard.Domain.Enums;

namespace ArticleDashboard.Domain.Entities;

public class Article
{
    public Guid Id { get; set; }
    public int ArticleNumber { get; set; }
    public string Name { get; set; } = string.Empty;
    public ArticleCategory ArticleCategory { get; set; }
    public Material Material { get; set; }
    public double NetWeight { get; set; }
    public double Length { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }

    public ICollection<BicycleCategory> BicycleCategories { get; set; } = new List<BicycleCategory>();
}