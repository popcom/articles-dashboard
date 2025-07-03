using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ArticleDashboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateForSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ArticleBicycleCategory",
                keyColumns: new[] { "ArticlesId", "BicycleCategoriesId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), 1 });

            migrationBuilder.DeleteData(
                table: "ArticleBicycleCategory",
                keyColumns: new[] { "ArticlesId", "BicycleCategoriesId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), 2 });

            migrationBuilder.DeleteData(
                table: "ArticleBicycleCategory",
                keyColumns: new[] { "ArticlesId", "BicycleCategoriesId" },
                keyValues: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), 2 });

            migrationBuilder.DeleteData(
                table: "ArticleBicycleCategory",
                keyColumns: new[] { "ArticlesId", "BicycleCategoriesId" },
                keyValues: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), 3 });

            migrationBuilder.DeleteData(
                table: "BicycleCategories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "BicycleCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BicycleCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BicycleCategories",
                keyColumn: "Id",
                keyValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "ArticleCategory", "ArticleNumber", "Height", "Length", "Material", "Name", "NetWeight", "Width" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Pedal", 1001, 0.0, 0.0, "Carbon", "Carbon Pedal X1", 620.0, 0.0 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Crank", 1002, 0.0, 0.0, "Aluminum", "Crank Pro 3000", 540.0, 0.0 }
                });

            migrationBuilder.InsertData(
                table: "BicycleCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Road" },
                    { 2, "Mountain" },
                    { 3, "Hybrid" },
                    { 4, "Electric" }
                });

            migrationBuilder.InsertData(
                table: "ArticleBicycleCategory",
                columns: new[] { "ArticlesId", "BicycleCategoriesId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { new Guid("11111111-1111-1111-1111-111111111111"), 2 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 2 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 3 }
                });
        }
    }
}
