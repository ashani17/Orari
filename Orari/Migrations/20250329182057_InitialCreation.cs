using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orari.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Credits = table.Column<int>(type: "int", nullable: false),
                    PId = table.Column<int>(type: "int", nullable: false),
                    Professor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CId);
                });

            migrationBuilder.CreateTable(
                name: "Professors",
                columns: table => new
                {
                    PId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Availability = table.Column<bool>(type: "bit", nullable: false),
                    SpecialRequirements = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professors", x => x.PId);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    RoomDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RId);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    SId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.SId);
                });

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    EId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CId = table.Column<int>(type: "int", nullable: false),
                    ExamDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CourseCId = table.Column<int>(type: "int", nullable: false),
                    SCId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.EId);
                    table.ForeignKey(
                        name: "FK_Exams_Courses_CourseCId",
                        column: x => x.CourseCId,
                        principalTable: "Courses",
                        principalColumn: "CId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    EId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SId = table.Column<int>(type: "int", nullable: false),
                    CId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.EId);
                    table.ForeignKey(
                        name: "FK_Enrollments_Courses_CId",
                        column: x => x.CId,
                        principalTable: "Courses",
                        principalColumn: "CId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollments_Students_SId",
                        column: x => x.SId,
                        principalTable: "Students",
                        principalColumn: "SId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    SCId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    RId = table.Column<int>(type: "int", nullable: false),
                    Room = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PId = table.Column<int>(type: "int", nullable: false),
                    Profesor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CId = table.Column<int>(type: "int", nullable: false),
                    Course = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.SCId);
                    table.ForeignKey(
                        name: "FK_Schedules_Exams_EId",
                        column: x => x.EId,
                        principalTable: "Exams",
                        principalColumn: "EId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_CId",
                table: "Enrollments",
                column: "CId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_SId",
                table: "Enrollments",
                column: "SId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_CourseCId",
                table: "Exams",
                column: "CourseCId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_EId",
                table: "Schedules",
                column: "EId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "Professors");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}
