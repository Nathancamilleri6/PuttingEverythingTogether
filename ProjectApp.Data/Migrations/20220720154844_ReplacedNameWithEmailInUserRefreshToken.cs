using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectApp.Data.Migrations
{
    public partial class ReplacedNameWithEmailInUserRefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "UserRefreshTokens",
                newName: "Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "UserRefreshTokens",
                newName: "UserName");
        }
    }
}
