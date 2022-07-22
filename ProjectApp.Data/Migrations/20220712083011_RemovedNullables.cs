using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectApp.Data.Migrations
{
    public partial class RemovedNullables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_CommentatorId",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "CommentatorId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_CommentatorId",
                table: "Comments",
                column: "CommentatorId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_CommentatorId",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "CommentatorId",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_CommentatorId",
                table: "Comments",
                column: "CommentatorId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
