using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Master.Microservice.Migrations
{
    /// <inheritdoc />
    public partial class NewMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    FName = table.Column<string>(type: "text", nullable: false),
                    StatusID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Name_en = table.Column<string>(type: "text", nullable: true),
                    Name_ar = table.Column<string>(type: "text", nullable: true),
                    CurrencyCodeID = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Countries_Currencies_CurrencyCodeID",
                        column: x => x.CurrencyCodeID,
                        principalTable: "Currencies",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Name_en = table.Column<string>(type: "text", nullable: true),
                    Name_ar = table.Column<string>(type: "text", nullable: true),
                    CountryIDId = table.Column<int>(type: "integer", nullable: true),
                    StatusId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Regions_Countries_CountryIDId",
                        column: x => x.CountryIDId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Name_en = table.Column<string>(type: "text", nullable: true),
                    Name_ar = table.Column<string>(type: "text", nullable: true),
                    CountryIDId = table.Column<int>(type: "integer", nullable: true),
                    RegionIDId = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Countries_CountryIDId",
                        column: x => x.CountryIDId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cities_Regions_RegionIDId",
                        column: x => x.RegionIDId,
                        principalTable: "Regions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryIDId",
                table: "Cities",
                column: "CountryIDId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_RegionIDId",
                table: "Cities",
                column: "RegionIDId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_CurrencyCodeID",
                table: "Countries",
                column: "CurrencyCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_CountryIDId",
                table: "Regions",
                column: "CountryIDId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
