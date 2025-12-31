using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OracleScry.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCardImportTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardImports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TotalCardsInFile = table.Column<int>(type: "int", nullable: false),
                    CardsProcessed = table.Column<int>(type: "int", nullable: false),
                    CardsAdded = table.Column<int>(type: "int", nullable: false),
                    CardsUpdated = table.Column<int>(type: "int", nullable: false),
                    CardsSkipped = table.Column<int>(type: "int", nullable: false),
                    CardsFailed = table.Column<int>(type: "int", nullable: false),
                    BulkDataId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DownloadUri = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ScryfallUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ImportedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardImports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardImportErrors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardImportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OracleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CardName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    StackTrace = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ImportedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardImportErrors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardImportErrors_CardImports_CardImportId",
                        column: x => x.CardImportId,
                        principalTable: "CardImports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardImportErrors_CardImportId",
                table: "CardImportErrors",
                column: "CardImportId");

            migrationBuilder.CreateIndex(
                name: "IX_CardImportErrors_OracleId",
                table: "CardImportErrors",
                column: "OracleId");

            migrationBuilder.CreateIndex(
                name: "IX_CardImports_ScryfallUpdatedAt",
                table: "CardImports",
                column: "ScryfallUpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CardImports_StartedAt",
                table: "CardImports",
                column: "StartedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_CardImports_Status",
                table: "CardImports",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardImportErrors");

            migrationBuilder.DropTable(
                name: "CardImports");
        }
    }
}
