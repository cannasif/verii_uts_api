using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace uts_api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_CUSTOMERS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TaxOffice = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    TaxNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TcknNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    District = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BranchCode = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    BusinessUnitCode = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    IsErpIntegrated = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ErpIntegrationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastSyncDateUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_RII_CUSTOMERS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RiiFnCariRow",
                columns: table => new
                {
                    SubeKodu = table.Column<short>(type: "smallint", nullable: false),
                    IsletmeKodu = table.Column<short>(type: "smallint", nullable: false),
                    CariKod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CariIsim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CariTel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CariIl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CariAdres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CariIlce = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UlkeKodu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Web = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VergiNumarasi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VergiDairesi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TcknNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CustomerCode",
                table: "RII_CUSTOMERS",
                column: "CustomerCode");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CustomerName",
                table: "RII_CUSTOMERS",
                column: "CustomerName");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_IsDeleted",
                table: "RII_CUSTOMERS",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_CUSTOMERS");

            migrationBuilder.DropTable(
                name: "RiiFnCariRow");
        }
    }
}
