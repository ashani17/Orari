using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orari.Migrations
{
    /// <inheritdoc />
    public partial class AddedServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Professors_ProfesorPId",
                table: "Exams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Professors",
                table: "Professors");

            migrationBuilder.RenameTable(
                name: "Professors",
                newName: "Profesors");

            migrationBuilder.RenameColumn(
                name: "Professor",
                table: "Courses",
                newName: "Profesor");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTime",
                table: "Exams",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "RId",
                table: "Exams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoomRId",
                table: "Exams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "StartTime",
                table: "Exams",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AlterColumn<string>(
                name: "PEmail",
                table: "Profesors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Profesors",
                table: "Profesors",
                column: "PId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_RoomRId",
                table: "Exams",
                column: "RoomRId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Profesors_ProfesorPId",
                table: "Exams",
                column: "ProfesorPId",
                principalTable: "Profesors",
                principalColumn: "PId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Rooms_RoomRId",
                table: "Exams",
                column: "RoomRId",
                principalTable: "Rooms",
                principalColumn: "RId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Profesors_ProfesorPId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Rooms_RoomRId",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Exams_RoomRId",
                table: "Exams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Profesors",
                table: "Profesors");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "RId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "RoomRId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Exams");

            migrationBuilder.RenameTable(
                name: "Profesors",
                newName: "Professors");

            migrationBuilder.RenameColumn(
                name: "Profesor",
                table: "Courses",
                newName: "Professor");

            migrationBuilder.AlterColumn<string>(
                name: "PEmail",
                table: "Professors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Professors",
                table: "Professors",
                column: "PId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Professors_ProfesorPId",
                table: "Exams",
                column: "ProfesorPId",
                principalTable: "Professors",
                principalColumn: "PId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
