using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace uts_api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStockAndHangfireMonitoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_STOCK",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ErpStockCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StockName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UreticiKodu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GrupKodu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GrupAdi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Kod1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Kod1Adi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Kod2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Kod2Adi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Kod3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Kod3Adi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Kod4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Kod4Adi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Kod5 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Kod5Adi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    BranchCode = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreateUser = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateUser = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteUser = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeletedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_STOCK", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RiiFnStockRow",
                columns: table => new
                {
                    SubeKodu = table.Column<short>(type: "smallint", nullable: false),
                    IsletmeKodu = table.Column<short>(type: "smallint", nullable: false),
                    StokKodu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OlcuBr1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UreticiKodu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StokAdi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrupKodu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrupIsim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kod1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kod1Adi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kod2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kod2Adi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kod3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kod3Adi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kod4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kod4Adi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kod5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kod5Adi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IngIsim = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stock_ErpStockCode",
                table: "RII_STOCK",
                column: "ErpStockCode");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_IsDeleted",
                table: "RII_STOCK",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_StockName",
                table: "RII_STOCK",
                column: "StockName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_STOCK");

            migrationBuilder.DropTable(
                name: "RiiFnStockRow");
        }
    }
}
