using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace uts_api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUretimTarihiModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_URETIM_TARIHI",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WfState = table.Column<byte>(type: "tinyint", nullable: true),
                    StokKodu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SeriLotNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LotNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SonKulTarih = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SYedek1 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SYedek2 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DYedek1 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IYedek1 = table.Column<int>(type: "int", nullable: true),
                    IYedek2 = table.Column<int>(type: "int", nullable: true),
                    FYedek1 = table.Column<double>(type: "float", nullable: true),
                    FYedek2 = table.Column<double>(type: "float", nullable: true),
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
                    table.PrimaryKey("PK_RII_URETIM_TARIHI", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UretimTarihi_IsDeleted",
                table: "RII_URETIM_TARIHI",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_UretimTarihi_LotNo",
                table: "RII_URETIM_TARIHI",
                column: "LotNo");

            migrationBuilder.CreateIndex(
                name: "IX_UretimTarihi_SeriLotNo",
                table: "RII_URETIM_TARIHI",
                column: "SeriLotNo");

            migrationBuilder.CreateIndex(
                name: "IX_UretimTarihi_StokKodu",
                table: "RII_URETIM_TARIHI",
                column: "StokKodu");

            migrationBuilder.CreateIndex(
                name: "IX_UretimTarihi_Tarih",
                table: "RII_URETIM_TARIHI",
                column: "Tarih");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_URETIM_TARIHI");
        }
    }
}
