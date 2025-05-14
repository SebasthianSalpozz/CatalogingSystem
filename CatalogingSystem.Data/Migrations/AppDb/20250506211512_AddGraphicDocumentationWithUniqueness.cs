using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogingSystem.Data.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class AddGraphicDocumentationWithUniqueness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GraphicDocumentations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    expediente = table.Column<long>(type: "bigint", nullable: false),
                    inventory = table.Column<long>(type: "bigint", nullable: false),
                    genericControlNumber = table.Column<string>(type: "text", nullable: true),
                    specificControlNumber = table.Column<string>(type: "text", nullable: true),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    supportTypes = table.Column<List<string>>(type: "text[]", nullable: true),
                    dimensions_Width = table.Column<double>(type: "double precision", nullable: true),
                    dimensions_Height = table.Column<double>(type: "double precision", nullable: true),
                    imageAuthor_FirstName = table.Column<string>(type: "text", nullable: true),
                    imageAuthor_LastName = table.Column<string>(type: "text", nullable: true),
                    imageAuthor_IdentityCard = table.Column<string>(type: "text", nullable: true),
                    imageAuthor_InstitutionalId = table.Column<string>(type: "text", nullable: true),
                    imageAuthor_Institution = table.Column<string>(type: "text", nullable: true),
                    imageAuthor_Address = table.Column<string>(type: "text", nullable: true),
                    imageAuthor_Locality = table.Column<string>(type: "text", nullable: true),
                    imageAuthor_Province = table.Column<string>(type: "text", nullable: true),
                    imageAuthor_Department = table.Column<string>(type: "text", nullable: true),
                    imageAuthor_Country = table.Column<string>(type: "text", nullable: true),
                    imageAuthor_PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    imageAuthor_Email = table.Column<string>(type: "text", nullable: true),
                    imageAuthor_References = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: false),
                    technicalData = table.Column<string>(type: "text", nullable: false),
                    generalObservations = table.Column<string>(type: "text", nullable: true),
                    imageUrls = table.Column<List<string>>(type: "text[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphicDocumentations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GraphicDocumentations_ArchivosAdministrativos_expediente",
                        column: x => x.expediente,
                        principalTable: "ArchivosAdministrativos",
                        principalColumn: "expediente",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GraphicDocumentations_expediente_inventory",
                table: "GraphicDocumentations",
                columns: new[] { "expediente", "inventory" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GraphicDocumentations");
        }
    }
}
