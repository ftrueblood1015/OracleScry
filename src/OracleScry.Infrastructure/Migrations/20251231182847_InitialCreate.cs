using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OracleScry.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScryfallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Object = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Uri = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ScryfallUri = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PrintsSearchUri = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RulingsUri = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ArenaId = table.Column<int>(type: "int", nullable: true),
                    MtgoId = table.Column<int>(type: "int", nullable: true),
                    MtgoFoilId = table.Column<int>(type: "int", nullable: true),
                    MultiverseIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TcgplayerId = table.Column<int>(type: "int", nullable: true),
                    TcgplayerEtchedId = table.Column<int>(type: "int", nullable: true),
                    CardmarketId = table.Column<int>(type: "int", nullable: true),
                    OracleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Lang = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Layout = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManaCost = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Cmc = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    TypeLine = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OracleText = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Power = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Toughness = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Loyalty = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Defense = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Colors = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColorIdentity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColorIndicator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProducedMana = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Keywords = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reserved = table.Column<bool>(type: "bit", nullable: false),
                    EdhrecRank = table.Column<int>(type: "int", nullable: true),
                    PennyRank = table.Column<int>(type: "int", nullable: true),
                    GameChanger = table.Column<bool>(type: "bit", nullable: true),
                    HandModifier = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LifeModifier = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Artist = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ArtistIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Booster = table.Column<bool>(type: "bit", nullable: false),
                    BorderColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CardBackId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CollectorNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ContentWarning = table.Column<bool>(type: "bit", nullable: true),
                    Digital = table.Column<bool>(type: "bit", nullable: false),
                    Finishes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlavorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FlavorText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FrameEffects = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Frame = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FullArt = table.Column<bool>(type: "bit", nullable: false),
                    Games = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HighresImage = table.Column<bool>(type: "bit", nullable: false),
                    IllustrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ImageStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Oversized = table.Column<bool>(type: "bit", nullable: false),
                    PrintedName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PrintedText = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PrintedTypeLine = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Promo = table.Column<bool>(type: "bit", nullable: false),
                    PromoTypes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rarity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReleasedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reprint = table.Column<bool>(type: "bit", nullable: false),
                    SetCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SetName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SetType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SetUri = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SetSearchUri = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ScryfallSetUri = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    StorySpotlight = table.Column<bool>(type: "bit", nullable: false),
                    Textless = table.Column<bool>(type: "bit", nullable: false),
                    Variation = table.Column<bool>(type: "bit", nullable: false),
                    VariationOf = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Watermark = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AttractionLights = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseUris = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelatedUris = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImportedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImageUris = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Legalities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Preview = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prices = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardFaces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Object = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ManaCost = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TypeLine = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OracleText = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Colors = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColorIndicator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Power = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Toughness = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Loyalty = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Defense = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    FlavorText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IllustrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Cmc = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    OracleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Layout = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PrintedName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PrintedText = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PrintedTypeLine = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Watermark = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Artist = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ArtistId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FaceIndex = table.Column<int>(type: "int", nullable: false),
                    ImageUris = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardFaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardFaces_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RelatedCards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Object = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RelatedCardScryfallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Component = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    TypeLine = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Uri = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelatedCards_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DisplayName",
                table: "AspNetUsers",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CardFaces_CardId",
                table: "CardFaces",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardFaces_Name",
                table: "CardFaces",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_Cmc",
                table: "Cards",
                column: "Cmc");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_Name",
                table: "Cards",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_OracleId",
                table: "Cards",
                column: "OracleId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_Rarity",
                table: "Cards",
                column: "Rarity");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_ReleasedAt",
                table: "Cards",
                column: "ReleasedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_ScryfallId",
                table: "Cards",
                column: "ScryfallId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_SetCode",
                table: "Cards",
                column: "SetCode");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_TypeLine",
                table: "Cards",
                column: "TypeLine");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedCards_CardId",
                table: "RelatedCards",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedCards_RelatedCardScryfallId",
                table: "RelatedCards",
                column: "RelatedCardScryfallId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CardFaces");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "RelatedCards");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Cards");
        }
    }
}
