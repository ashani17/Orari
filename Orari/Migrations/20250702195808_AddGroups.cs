using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orari.Migrations
{
    /// <inheritdoc />
    public partial class AddGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "Schedules",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Group",
                table: "Schedules");
        }
    }
}
