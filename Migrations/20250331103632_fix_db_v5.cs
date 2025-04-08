using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace my_cosmetic_store.Migrations
{
    /// <inheritdoc />
    public partial class fix_db_v5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_Items_Product_Variants_VariantId",
                table: "cart_Items");

            migrationBuilder.RenameColumn(
                name: "VariantId",
                table: "cart_Items",
                newName: "ProductVariantId");

            migrationBuilder.RenameIndex(
                name: "IX_cart_Items_VariantId",
                table: "cart_Items",
                newName: "IX_cart_Items_ProductVariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_cart_Items_Product_Variants_ProductVariantId",
                table: "cart_Items",
                column: "ProductVariantId",
                principalTable: "Product_Variants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_Items_Product_Variants_ProductVariantId",
                table: "cart_Items");

            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "cart_Items",
                newName: "VariantId");

            migrationBuilder.RenameIndex(
                name: "IX_cart_Items_ProductVariantId",
                table: "cart_Items",
                newName: "IX_cart_Items_VariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_cart_Items_Product_Variants_VariantId",
                table: "cart_Items",
                column: "VariantId",
                principalTable: "Product_Variants",
                principalColumn: "Id");
        }
    }
}
