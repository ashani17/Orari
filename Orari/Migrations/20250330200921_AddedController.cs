using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orari.Migrations
{
    /// <inheritdoc />
    public partial class AddedController : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Students",
                newName: "SSurname");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Students",
                newName: "SPassword");

            migrationBuilder.RenameColumn(
                name: "RoomDescription",
                table: "Rooms",
                newName: "RType");

            migrationBuilder.RenameColumn(
                name: "Capacity",
                table: "Rooms",
                newName: "RCapacity");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Courses",
                newName: "CName");

            migrationBuilder.AddColumn<DateTime>(
                name: "SCreatedAt",
                table: "Students",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SEmail",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SName",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "SUpdatedAt",
                table: "Students",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RDescription",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PCreatedAt",
                table: "Professors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PEmail",
                table: "Professors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PPassword",
                table: "Professors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PPhone",
                table: "Professors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PSubject",
                table: "Professors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PSurname",
                table: "Professors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PUpdatedAt",
                table: "Professors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PId",
                table: "Exams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProfesorPId",
                table: "Exams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ProfesorPId",
                table: "Exams",
                column: "ProfesorPId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Professors_ProfesorPId",
                table: "Exams",
                column: "ProfesorPId",
                principalTable: "Professors",
                principalColumn: "PId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Professors_ProfesorPId",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Exams_ProfesorPId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "SCreatedAt",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "SEmail",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "SName",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "SUpdatedAt",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "RDescription",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "PCreatedAt",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "PEmail",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "PPassword",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "PPhone",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "PSubject",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "PSurname",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "PUpdatedAt",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "PId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "ProfesorPId",
                table: "Exams");

            migrationBuilder.RenameColumn(
                name: "SSurname",
                table: "Students",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "SPassword",
                table: "Students",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "RType",
                table: "Rooms",
                newName: "RoomDescription");

            migrationBuilder.RenameColumn(
                name: "RCapacity",
                table: "Rooms",
                newName: "Capacity");

            migrationBuilder.RenameColumn(
                name: "CName",
                table: "Courses",
                newName: "Name");
        }
    }
}
