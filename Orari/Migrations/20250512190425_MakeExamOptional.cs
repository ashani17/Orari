using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orari.Migrations
{
    /// <inheritdoc />
    public partial class MakeExamOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Exams_EId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_EId",
                table: "Schedules");

            migrationBuilder.AlterColumn<int>(
                name: "EId",
                table: "Schedules",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_EId",
                table: "Schedules",
                column: "EId",
                unique: true,
                filter: "[EId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Exams_EId",
                table: "Schedules",
                column: "EId",
                principalTable: "Exams",
                principalColumn: "EId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Exams_EId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_EId",
                table: "Schedules");

            migrationBuilder.AlterColumn<int>(
                name: "EId",
                table: "Schedules",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_EId",
                table: "Schedules",
                column: "EId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Exams_EId",
                table: "Schedules",
                column: "EId",
                principalTable: "Exams",
                principalColumn: "EId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
