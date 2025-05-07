using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogingSystem.Data.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class GraphicDocumentationFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GraphicDocumentations_expediente_inventory",
                table: "GraphicDocumentations");

            migrationBuilder.CreateIndex(
                name: "IX_GraphicDocumentations_expediente",
                table: "GraphicDocumentations",
                column: "expediente",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GraphicDocumentations_expediente",
                table: "GraphicDocumentations");

            migrationBuilder.CreateIndex(
                name: "IX_GraphicDocumentations_expediente_inventory",
                table: "GraphicDocumentations",
                columns: new[] { "expediente", "inventory" },
                unique: true);
        }
    }
}
