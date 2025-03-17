using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace my_cosmetic_store.Migrations
{
    /// <inheritdoc />
    public partial class fixdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Product_Images_ProductID",
                table: "Product_Images",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Images_Products_ProductID",
                table: "Product_Images",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Images_Products_ProductID",
                table: "Product_Images");

            migrationBuilder.DropIndex(
                name: "IX_Product_Images_ProductID",
                table: "Product_Images");
        }
    }
}
