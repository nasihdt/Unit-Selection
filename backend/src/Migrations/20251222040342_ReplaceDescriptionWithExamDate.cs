using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityRegistration.Api.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceDescriptionWithExamDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Courses");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExamDate",
                table: "Courses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamDate",
                table: "Courses");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Courses",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
