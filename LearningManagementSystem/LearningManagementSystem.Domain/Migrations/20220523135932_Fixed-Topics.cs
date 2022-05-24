using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Domain.Migrations
{
    public partial class FixedTopics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomeTasks_Topics_Id",
                table: "HomeTasks");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "HomeTasks",
                newName: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_HomeTasks_Topics_TopicId",
                table: "HomeTasks",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomeTasks_Topics_TopicId",
                table: "HomeTasks");

            migrationBuilder.RenameColumn(
                name: "TopicId",
                table: "HomeTasks",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HomeTasks_Topics_Id",
                table: "HomeTasks",
                column: "Id",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
