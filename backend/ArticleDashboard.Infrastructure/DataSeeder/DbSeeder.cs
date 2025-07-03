using ArticleDashboard.Application.DTOs;
using ArticleDashboard.Domain.Entities;
using ArticleDashboard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArticleDashboard.Infrastructure.DataSeeder;
public static class DbSeeder
{
    public static async Task SeedBicycleCategoriesAsync(ApplicationDbContext context)
    {
        if (context.BicycleCategories.Any()) return;

        var path = Path.Combine(AppContext.BaseDirectory, "DataSeeder", "bicycle-categories.json");
        var json = await File.ReadAllTextAsync(path);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var categories = JsonSerializer.Deserialize<List<BicycleCategory>>(json, options)!;

        context.BicycleCategories.AddRange(categories);
        await context.SaveChangesAsync();
    }

    public static async Task SeedArticlesAsync(ApplicationDbContext context)
    {
        if (context.Articles.Any()) return;

        var jsonPath = Path.Combine(AppContext.BaseDirectory, "DataSeeder", "articles.json");
        var json = await File.ReadAllTextAsync(jsonPath);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };
        var articlesToSeed = JsonSerializer.Deserialize<List<ArticleSeedDto>>(json, options)!;

        foreach (var model in articlesToSeed)
        {
            var categories = await context.BicycleCategories
                .Where(c => model.BicycleCategoryIds.Contains(c.Id))
                .ToListAsync();

            var article = new Article
            {
                ArticleNumber = model.ArticleNumber,
                Name = model.Name,
                ArticleCategory = model.ArticleCategory,
                Material = model.Material,
                NetWeight = model.NetWeight,
                BicycleCategories = categories
            };

            context.Articles.Add(article);
        }

        await context.SaveChangesAsync();
    }
}
