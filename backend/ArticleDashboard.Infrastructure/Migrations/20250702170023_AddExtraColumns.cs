using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArticleDashboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExtraColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArticleNumber",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Height",
                table: "Articles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Length",
                table: "Articles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Width",
                table: "Articles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticleNumber",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Articles");
        }
    }
}
