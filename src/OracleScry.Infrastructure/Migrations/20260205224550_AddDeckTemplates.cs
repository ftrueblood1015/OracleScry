using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OracleScry.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDeckTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeckTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Format = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SetCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SetName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ScryfallDeckId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReleasedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ImportedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeckTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeckTemplateCards",
                columns: table => new
                {
                    DeckTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsSideboard = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsCommander = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeckTemplateCards", x => new { x.DeckTemplateId, x.CardId });
                    table.ForeignKey(
                        name: "FK_DeckTemplateCards_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeckTemplateCards_DeckTemplates_DeckTemplateId",
                        column: x => x.DeckTemplateId,
                        principalTable: "DeckTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeckTemplateCards_CardId",
                table: "DeckTemplateCards",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_DeckTemplateCards_DeckTemplateId",
                table: "DeckTemplateCards",
                column: "DeckTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_DeckTemplateCards_IsCommander",
                table: "DeckTemplateCards",
                column: "IsCommander");

            migrationBuilder.CreateIndex(
                name: "IX_DeckTemplateCards_IsSideboard",
                table: "DeckTemplateCards",
                column: "IsSideboard");

            migrationBuilder.CreateIndex(
                name: "IX_DeckTemplates_Format",
                table: "DeckTemplates",
                column: "Format");

            migrationBuilder.CreateIndex(
                name: "IX_DeckTemplates_IsActive",
                table: "DeckTemplates",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_DeckTemplates_Name",
                table: "DeckTemplates",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_DeckTemplates_SetCode",
                table: "DeckTemplates",
                column: "SetCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeckTemplateCards");

            migrationBuilder.DropTable(
                name: "DeckTemplates");
        }
    }
}
