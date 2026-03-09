using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace uts_api.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddHangfireMonitoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_HANGFIRE_JOB_LOGS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    JobName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OccurredAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ExceptionType = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    StackTrace = table.Column<string>(type: "nvarchar(max)", maxLength: 8000, nullable: true),
                    Queue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_RII_HANGFIRE_JOB_LOGS", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HangfireJobLog_JobId",
                table: "RII_HANGFIRE_JOB_LOGS",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireJobLog_JobName",
                table: "RII_HANGFIRE_JOB_LOGS",
                column: "JobName");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireJobLog_OccurredAtUtc",
                table: "RII_HANGFIRE_JOB_LOGS",
                column: "OccurredAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireJobLog_State",
                table: "RII_HANGFIRE_JOB_LOGS",
                column: "State");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_HANGFIRE_JOB_LOGS");
        }
    }
}
