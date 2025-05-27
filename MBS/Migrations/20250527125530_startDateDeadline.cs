using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MBS.Migrations
{
    /// <inheritdoc />
    public partial class startDateDeadline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "TodoTasks",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "TodoTasks");
        }
    }
}
