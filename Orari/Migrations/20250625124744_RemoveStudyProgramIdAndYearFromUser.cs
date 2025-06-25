using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orari.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStudyProgramIdAndYearFromUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropColumn(
            //     name: "StudyProgramId",
            //     table: "AspNetUsers");

            // migrationBuilder.DropColumn(
            //     name: "Year",
            //     table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudyProgramId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }
    }
}
