using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orari.Migrations
{
    /// <inheritdoc />
    public partial class DepartmentsStudyProgram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DId);
                });

            migrationBuilder.CreateTable(
                name: "StudyPrograms",
                columns: table => new
                {
                    SPId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SPName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyPrograms", x => x.SPId);
                    table.ForeignKey(
                        name: "FK_StudyPrograms_Departments_DId",
                        column: x => x.DId,
                        principalTable: "Departments",
                        principalColumn: "DId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudyProgramCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SPId = table.Column<int>(type: "int", nullable: false),
                    CId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyProgramCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyProgramCourses_Courses_CId",
                        column: x => x.CId,
                        principalTable: "Courses",
                        principalColumn: "CId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudyProgramCourses_StudyPrograms_SPId",
                        column: x => x.SPId,
                        principalTable: "StudyPrograms",
                        principalColumn: "SPId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudyProgramCourses_CId",
                table: "StudyProgramCourses",
                column: "CId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyProgramCourses_SPId",
                table: "StudyProgramCourses",
                column: "SPId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPrograms_DId",
                table: "StudyPrograms",
                column: "DId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudyProgramCourses");

            migrationBuilder.DropTable(
                name: "StudyPrograms");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
