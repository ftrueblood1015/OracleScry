using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OracleScry.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTypeConstraintsToCardPurpose : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExcludedTypes",
                table: "CardPurposes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequiredTypes",
                table: "CardPurposes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExcludedTypes",
                table: "CardPurposes");

            migrationBuilder.DropColumn(
                name: "RequiredTypes",
                table: "CardPurposes");
        }
    }
}
