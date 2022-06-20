using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Domain.Migrations
{
    public partial class UpdatedChat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupChatMessage_Groups_GroupId",
                table: "GroupChatMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupChatMessage_Users_SenderId",
                table: "GroupChatMessage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupChatMessage",
                table: "GroupChatMessage");

            migrationBuilder.RenameTable(
                name: "GroupChatMessage",
                newName: "GroupChatMessages");

            migrationBuilder.RenameIndex(
                name: "IX_GroupChatMessage_SenderId",
                table: "GroupChatMessages",
                newName: "IX_GroupChatMessages_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupChatMessage_GroupId",
                table: "GroupChatMessages",
                newName: "IX_GroupChatMessages_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupChatMessages",
                table: "GroupChatMessages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChatMessages_Groups_GroupId",
                table: "GroupChatMessages",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChatMessages_Users_SenderId",
                table: "GroupChatMessages",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupChatMessages_Groups_GroupId",
                table: "GroupChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupChatMessages_Users_SenderId",
                table: "GroupChatMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupChatMessages",
                table: "GroupChatMessages");

            migrationBuilder.RenameTable(
                name: "GroupChatMessages",
                newName: "GroupChatMessage");

            migrationBuilder.RenameIndex(
                name: "IX_GroupChatMessages_SenderId",
                table: "GroupChatMessage",
                newName: "IX_GroupChatMessage_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupChatMessages_GroupId",
                table: "GroupChatMessage",
                newName: "IX_GroupChatMessage_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupChatMessage",
                table: "GroupChatMessage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChatMessage_Groups_GroupId",
                table: "GroupChatMessage",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChatMessage_Users_SenderId",
                table: "GroupChatMessage",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
