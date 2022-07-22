using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectApp.Data.Migrations
{
    public partial class RemovedDescriptionPropertyFromProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Projects");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
