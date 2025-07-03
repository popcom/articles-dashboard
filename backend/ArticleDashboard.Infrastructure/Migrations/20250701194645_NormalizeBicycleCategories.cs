using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ArticleDashboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeBicycleCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BicycleCategories",
                table: "Articles");

            migrationBuilder.AlterColumn<double>(
                name: "NetWeight",
                table: "Articles",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.CreateTable(
                name: "BicycleCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BicycleCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArticleBicycleCategory",
                columns: table => new
                {
                    ArticlesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BicycleCategoriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleBicycleCategory", x => new { x.ArticlesId, x.BicycleCategoriesId });
                    table.ForeignKey(
                        name: "FK_ArticleBicycleCategory_Articles_ArticlesId",
                        column: x => x.ArticlesId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleBicycleCategory_BicycleCategories_BicycleCategoriesId",
                        column: x => x.BicycleCategoriesId,
                        principalTable: "BicycleCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleBicycleCategory_BicycleCategoriesId",
                table: "ArticleBicycleCategory",
                column: "BicycleCategoriesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleBicycleCategory");

            migrationBuilder.DropTable(
                name: "BicycleCategories");

            migrationBuilder.AlterColumn<float>(
                name: "NetWeight",
                table: "Articles",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "BicycleCategories",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
