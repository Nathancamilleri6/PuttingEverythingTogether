using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectApp.Data.Migrations
{
    public partial class AlterationsForAssignees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_Assignee_Projects_ProjectId",
                table: "Project_Assignee");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Assignee_Users_AssigneeId",
                table: "Project_Assignee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Project_Assignee",
                table: "Project_Assignee");

            migrationBuilder.RenameTable(
                name: "Project_Assignee",
                newName: "Assignees");

            migrationBuilder.RenameIndex(
                name: "IX_Project_Assignee_ProjectId",
                table: "Assignees",
                newName: "IX_Assignees_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Project_Assignee_AssigneeId",
                table: "Assignees",
                newName: "IX_Assignees_AssigneeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assignees",
                table: "Assignees",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignees_Projects_ProjectId",
                table: "Assignees",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignees_Users_AssigneeId",
                table: "Assignees",
                column: "AssigneeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignees_Projects_ProjectId",
                table: "Assignees");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignees_Users_AssigneeId",
                table: "Assignees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assignees",
                table: "Assignees");

            migrationBuilder.RenameTable(
                name: "Assignees",
                newName: "Project_Assignee");

            migrationBuilder.RenameIndex(
                name: "IX_Assignees_ProjectId",
                table: "Project_Assignee",
                newName: "IX_Project_Assignee_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignees_AssigneeId",
                table: "Project_Assignee",
                newName: "IX_Project_Assignee_AssigneeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Project_Assignee",
                table: "Project_Assignee",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Assignee_Projects_ProjectId",
                table: "Project_Assignee",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Assignee_Users_AssigneeId",
                table: "Project_Assignee",
                column: "AssigneeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
