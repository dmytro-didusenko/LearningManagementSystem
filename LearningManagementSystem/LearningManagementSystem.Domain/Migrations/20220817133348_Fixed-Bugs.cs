using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Domain.Migrations
{
    public partial class FixedBugs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffChatMessage_Users_SenderId",
                table: "StaffChatMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Role_RoleId",
                table: "Users");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Role_TempId1",
                table: "Role");

            migrationBuilder.RenameTable(
                name: "StaffChatMessage",
                newName: "StaffChatMessages");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "Roles");

            migrationBuilder.RenameColumn(
                name: "TempId1",
                table: "Roles",
                newName: "Id");

            migrationBuilder.AlterColumn<Guid>(
                name: "SenderId",
                table: "StaffChatMessages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "StaffChatMessages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "StaffChatMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "StaffChatMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StaffChatMessages",
                table: "StaffChatMessages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_StaffChatMessages_SenderId",
                table: "StaffChatMessages",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffChatMessages_Users_SenderId",
                table: "StaffChatMessages",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffChatMessages_Users_SenderId",
                table: "StaffChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StaffChatMessages",
                table: "StaffChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_StaffChatMessages_SenderId",
                table: "StaffChatMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StaffChatMessages");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "StaffChatMessages");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "StaffChatMessages");

            migrationBuilder.DropColumn(
                name: "RoleName",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "StaffChatMessages",
                newName: "StaffChatMessage");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Role",
                newName: "TempId1");

            migrationBuilder.AlterColumn<Guid>(
                name: "SenderId",
                table: "StaffChatMessage",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Role_TempId1",
                table: "Role",
                column: "TempId1");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffChatMessage_Users_SenderId",
                table: "StaffChatMessage",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Role_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "TempId1");
        }
    }
}
