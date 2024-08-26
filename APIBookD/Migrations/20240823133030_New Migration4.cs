using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIBookD.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BookRates",
                table: "BookRates");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "BookRates",
                newName: "BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookRates",
                table: "BookRates",
                columns: new[] { "BookId", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BookRates",
                table: "BookRates");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "BookRates",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookRates",
                table: "BookRates",
                column: "Id");
        }
    }
}
