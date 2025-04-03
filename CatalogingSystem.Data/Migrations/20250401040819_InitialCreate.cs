using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogingSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArchivosAdministrativos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    institucion = table.Column<string>(type: "text", nullable: false),
                    unidad = table.Column<string>(type: "text", nullable: false),
                    expediente = table.Column<long>(type: "bigint", nullable: false),
                    serie = table.Column<string>(type: "text", nullable: true),
                    documentoOrigen = table.Column<string>(type: "text", nullable: false),
                    fechaInicial = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fechaFinal = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    expedienteAnterior = table.Column<string>(type: "text", nullable: true),
                    asunto = table.Column<string>(type: "text", nullable: true),
                    peticionTransferencia = table.Column<bool>(type: "boolean", nullable: true),
                    historial = table.Column<string>(type: "text", nullable: true),
                    archivoDocumental = table.Column<string>(type: "text", nullable: true),
                    observaciones = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchivosAdministrativos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArchivosAdministrativos");
        }
    }
}
