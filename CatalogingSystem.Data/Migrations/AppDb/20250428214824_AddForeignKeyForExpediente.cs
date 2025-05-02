using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogingSystem.Data.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class AddForeignKeyForExpediente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_ArchivosAdministrativos_expediente",
                table: "ArchivosAdministrativos",
                column: "expediente");

            migrationBuilder.CreateIndex(
                name: "IX_Identifications_expediente",
                table: "Identifications",
                column: "expediente");

            migrationBuilder.CreateIndex(
                name: "IX_ArchivosAdministrativos_expediente",
                table: "ArchivosAdministrativos",
                column: "expediente",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Identifications_ArchivosAdministrativos_expediente",
                table: "Identifications",
                column: "expediente",
                principalTable: "ArchivosAdministrativos",
                principalColumn: "expediente",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Identifications_ArchivosAdministrativos_expediente",
                table: "Identifications");

            migrationBuilder.DropIndex(
                name: "IX_Identifications_expediente",
                table: "Identifications");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ArchivosAdministrativos_expediente",
                table: "ArchivosAdministrativos");

            migrationBuilder.DropIndex(
                name: "IX_ArchivosAdministrativos_expediente",
                table: "ArchivosAdministrativos");
        }
    }
}
