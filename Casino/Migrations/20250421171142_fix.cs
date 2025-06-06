using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Casino.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EvenOroD",
                table: "Bet",
                newName: "EvenOdd");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EvenOdd",
                table: "Bet",
                newName: "EvenOroD");
        }
    }
}
