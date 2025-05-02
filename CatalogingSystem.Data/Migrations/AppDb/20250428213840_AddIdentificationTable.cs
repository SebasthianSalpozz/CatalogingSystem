using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogingSystem.Data.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class AddIdentificationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Identifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    unit = table.Column<string>(type: "text", nullable: false),
                    section_Room = table.Column<string>(type: "text", nullable: false),
                    section_Panel = table.Column<string>(type: "text", nullable: true),
                    section_DisplayCase = table.Column<string>(type: "text", nullable: true),
                    section_Easel = table.Column<string>(type: "text", nullable: true),
                    section_Storage = table.Column<string>(type: "text", nullable: true),
                    section_Courtyard = table.Column<string>(type: "text", nullable: true),
                    section_Pillar = table.Column<string>(type: "text", nullable: true),
                    section_Others = table.Column<string>(type: "text", nullable: true),
                    inventory = table.Column<long>(type: "bigint", nullable: false),
                    numberOfObjects = table.Column<int>(type: "integer", nullable: false),
                    genericClassification = table.Column<string>(type: "text", nullable: false),
                    objectName = table.Column<string>(type: "text", nullable: false),
                    typology_Type = table.Column<string>(type: "text", nullable: false),
                    typology_Subtype = table.Column<string>(type: "text", nullable: true),
                    typology_Class = table.Column<string>(type: "text", nullable: true),
                    typology_Subclass = table.Column<string>(type: "text", nullable: true),
                    typology_Order = table.Column<string>(type: "text", nullable: true),
                    typology_Suborder = table.Column<string>(type: "text", nullable: true),
                    specificName_GenericName = table.Column<string>(type: "text", nullable: false),
                    specificName_RelatedTerms = table.Column<string>(type: "text", nullable: true),
                    specificName_SpecificTerms = table.Column<string>(type: "text", nullable: true),
                    specificName_UsedBy = table.Column<string>(type: "text", nullable: true),
                    specificName_Notes = table.Column<string>(type: "text", nullable: true),
                    author_Name = table.Column<string>(type: "text", nullable: false),
                    author_BirthPlace = table.Column<string>(type: "text", nullable: false),
                    author_BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    author_DeathPlace = table.Column<string>(type: "text", nullable: true),
                    author_DeathDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    title_Name = table.Column<string>(type: "text", nullable: false),
                    title_Attribution = table.Column<string>(type: "text", nullable: true),
                    title_Translation = table.Column<string>(type: "text", nullable: true),
                    material_DescribedPart = table.Column<string>(type: "text", nullable: true),
                    material_MaterialName = table.Column<string>(type: "text", nullable: false),
                    material_Colors = table.Column<string>(type: "text", nullable: true),
                    techniques_DescribedPart = table.Column<string>(type: "text", nullable: true),
                    techniques_TechniqueName = table.Column<string>(type: "text", nullable: false),
                    observations = table.Column<string>(type: "text", nullable: false),
                    expediente = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identifications", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Identifications");
        }
    }
}
