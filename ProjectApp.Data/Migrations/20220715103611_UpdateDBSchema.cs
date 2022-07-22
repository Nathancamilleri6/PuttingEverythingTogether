using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectApp.Data.Migrations
{
    public partial class UpdateDBSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Tags_TagId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_AssigneeId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_AssigneeId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_TagId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AssigneeId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "CommentValue",
                table: "Comments",
                newName: "Value");

            migrationBuilder.CreateTable(
                name: "Project_Assignee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    AssigneeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Assignee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Project_Assignee_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Project_Assignee_Users_AssigneeId",
                        column: x => x.AssigneeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Project_Assignee_AssigneeId",
                table: "Project_Assignee",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Assignee_ProjectId",
                table: "Project_Assignee",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Project_Assignee");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Comments",
                newName: "CommentValue");

            migrationBuilder.AddColumn<int>(
                name: "AssigneeId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_AssigneeId",
                table: "Projects",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_TagId",
                table: "Projects",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Tags_TagId",
                table: "Projects",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_AssigneeId",
                table: "Projects",
                column: "AssigneeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
