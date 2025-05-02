using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogingSystem.Data.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class RemoveForeignKeyForExpediente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "unit",
                table: "Identifications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "unit",
                table: "Identifications",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
