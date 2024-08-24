using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBazaar.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateOrderHeader2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "OrderHeaders",
                newName: "lastName");

            migrationBuilder.AddColumn<string>(
                name: "firstName",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "firstName",
                table: "OrderHeaders");

            migrationBuilder.RenameColumn(
                name: "lastName",
                table: "OrderHeaders",
                newName: "Name");
        }
    }
}
