using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Domain.Migrations
{
    public partial class AddedTopicsHomeTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomeTasks_Subjects_SubjectId",
                table: "HomeTasks");

            migrationBuilder.DropIndex(
                name: "IX_HomeTasks_SubjectId",
                table: "HomeTasks");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "HomeTasks");

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfCreation = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Topics_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskAnswers_StudentId",
                table: "TaskAnswers",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_SubjectId",
                table: "Topics",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_HomeTasks_Topics_Id",
                table: "HomeTasks",
                column: "Id",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAnswers_Students_StudentId",
                table: "TaskAnswers",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomeTasks_Topics_Id",
                table: "HomeTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAnswers_Students_StudentId",
                table: "TaskAnswers");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_TaskAnswers_StudentId",
                table: "TaskAnswers");

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "HomeTasks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_HomeTasks_SubjectId",
                table: "HomeTasks",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_HomeTasks_Subjects_SubjectId",
                table: "HomeTasks",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
