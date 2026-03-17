using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace uts_api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUtsLogModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_UTS_LOG",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BNO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SIRA = table.Column<int>(type: "int", nullable: true),
                    SERITRAINC = table.Column<int>(type: "int", nullable: true),
                    STOK_KODU = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    SERI_NO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MIKTAR = table.Column<decimal>(type: "decimal(28,8)", precision: 28, scale: 8, nullable: true),
                    GONDERIM_TARIHI = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GONDEREN_KISI = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    GONDERIM_TIPI = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    SONUC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DURUM = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
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
                    table.PrimaryKey("PK_RII_UTS_LOG", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UtsLog_Bno",
                table: "RII_UTS_LOG",
                column: "BNO");

            migrationBuilder.CreateIndex(
                name: "IX_UtsLog_Durum",
                table: "RII_UTS_LOG",
                column: "DURUM");

            migrationBuilder.CreateIndex(
                name: "IX_UtsLog_GonderimTarihi",
                table: "RII_UTS_LOG",
                column: "GONDERIM_TARIHI");

            migrationBuilder.CreateIndex(
                name: "IX_UtsLog_IsDeleted",
                table: "RII_UTS_LOG",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_UtsLog_SeriNo",
                table: "RII_UTS_LOG",
                column: "SERI_NO");

            migrationBuilder.CreateIndex(
                name: "IX_UtsLog_StokKodu",
                table: "RII_UTS_LOG",
                column: "STOK_KODU");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_UTS_LOG");
        }
    }
}
