using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Domain.Migrations
{
    public partial class AddedTopics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomeTasks_Subjects_SubjectId",
                table: "HomeTasks");

            migrationBuilder.DropIndex(
                name: "IX_HomeTasks_SubjectId",
                table: "HomeTasks");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "HomeTasks",
                newName: "TopicId");

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateOfCreation = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Topics_HomeTasks_HomeTaskId",
                        column: x => x.HomeTaskId,
                        principalTable: "HomeTasks",
                        principalColumn: "Id");
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
                name: "IX_Topics_HomeTaskId",
                table: "Topics",
                column: "HomeTaskId",
                unique: true,
                filter: "[HomeTaskId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_SubjectId",
                table: "Topics",
                column: "SubjectId");

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
                name: "FK_TaskAnswers_Students_StudentId",
                table: "TaskAnswers");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_TaskAnswers_StudentId",
                table: "TaskAnswers");

            migrationBuilder.RenameColumn(
                name: "TopicId",
                table: "HomeTasks",
                newName: "SubjectId");

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
