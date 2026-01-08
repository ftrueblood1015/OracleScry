using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OracleScry.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCardPurposeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardPurposes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Patterns = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ImportedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardPurposes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurposeExtractionJobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TotalCards = table.Column<int>(type: "int", nullable: false),
                    ProcessedCards = table.Column<int>(type: "int", nullable: false),
                    PurposesAssigned = table.Column<int>(type: "int", nullable: false),
                    ErrorCount = table.Column<int>(type: "int", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ReprocessAll = table.Column<bool>(type: "bit", nullable: false),
                    ImportedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurposeExtractionJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardCardPurposes",
                columns: table => new
                {
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardPurposeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Confidence = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false),
                    MatchedPattern = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardCardPurposes", x => new { x.CardId, x.CardPurposeId });
                    table.ForeignKey(
                        name: "FK_CardCardPurposes_CardPurposes_CardPurposeId",
                        column: x => x.CardPurposeId,
                        principalTable: "CardPurposes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardCardPurposes_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardCardPurposes_AssignedAt",
                table: "CardCardPurposes",
                column: "AssignedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CardCardPurposes_CardId",
                table: "CardCardPurposes",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardCardPurposes_CardPurposeId",
                table: "CardCardPurposes",
                column: "CardPurposeId");

            migrationBuilder.CreateIndex(
                name: "IX_CardPurposes_Category",
                table: "CardPurposes",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_CardPurposes_DisplayOrder",
                table: "CardPurposes",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_CardPurposes_IsActive",
                table: "CardPurposes",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_CardPurposes_Slug",
                table: "CardPurposes",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurposeExtractionJobs_StartedAt",
                table: "PurposeExtractionJobs",
                column: "StartedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_PurposeExtractionJobs_Status",
                table: "PurposeExtractionJobs",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardCardPurposes");

            migrationBuilder.DropTable(
                name: "PurposeExtractionJobs");

            migrationBuilder.DropTable(
                name: "CardPurposes");
        }
    }
}
