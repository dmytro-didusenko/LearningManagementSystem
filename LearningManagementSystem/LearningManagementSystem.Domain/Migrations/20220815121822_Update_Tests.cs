using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.Domain.Migrations
{
    public partial class Update_Tests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Tests");

            migrationBuilder.AddColumn<int>(
                name: "DurationInMinutes",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationInMinutes",
                table: "Tests");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Tests",
                type: "time",
                nullable: true);
        }
    }
}
