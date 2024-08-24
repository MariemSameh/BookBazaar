using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBazaar.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Books_ProductId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_ProductId",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "OrderDetail");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_bookId",
                table: "OrderDetail",
                column: "bookId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Books_bookId",
                table: "OrderDetail",
                column: "bookId",
                principalTable: "Books",
                principalColumn: "bookId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Books_bookId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_bookId",
                table: "OrderDetail");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "OrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ProductId",
                table: "OrderDetail",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Books_ProductId",
                table: "OrderDetail",
                column: "ProductId",
                principalTable: "Books",
                principalColumn: "bookId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
