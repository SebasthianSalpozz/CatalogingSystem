using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogingSystem.Data.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class AddUserPermision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PermissionLevel",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermissionLevel",
                table: "AspNetUsers");
        }
    }
}
