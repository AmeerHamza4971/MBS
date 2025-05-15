using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MBS.Migrations
{
    /// <inheritdoc />
    public partial class changecolumnnameRemaing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remaing",
                table: "Billings");

            migrationBuilder.AddColumn<decimal>(
                name: "RemaingAmount",
                table: "Billings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemaingAmount",
                table: "Billings");

            migrationBuilder.AddColumn<string>(
                name: "Remaing",
                table: "Billings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
