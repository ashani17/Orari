using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orari.Migrations
{
    /// <inheritdoc />
    public partial class AddRecurringScheduleIdToSchedules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecurringScheduleId",
                table: "Schedules",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_RecurringScheduleId",
                table: "Schedules",
                column: "RecurringScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_RecurringSchedules_RecurringScheduleId",
                table: "Schedules",
                column: "RecurringScheduleId",
                principalTable: "RecurringSchedules",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_RecurringSchedules_RecurringScheduleId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_RecurringScheduleId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "RecurringScheduleId",
                table: "Schedules");
        }
    }
}
