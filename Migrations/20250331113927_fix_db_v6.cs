using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace my_cosmetic_store.Migrations
{
    /// <inheritdoc />
    public partial class fix_db_v6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_Items_Product_Variants_ProductVariantId",
                table: "cart_Items");

            migrationBuilder.DropIndex(
                name: "IX_Product_Variants_ProductID",
                table: "Product_Variants");

            migrationBuilder.DropIndex(
                name: "IX_cart_Items_ProductID",
                table: "cart_Items");

            migrationBuilder.DropIndex(
                name: "IX_cart_Items_ProductVariantId",
                table: "cart_Items");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Product_Variants_ProductID_VariantId",
                table: "Product_Variants",
                columns: new[] { "ProductID", "VariantId" });

            migrationBuilder.CreateIndex(
                name: "IX_cart_Items_ProductID_ProductVariantId",
                table: "cart_Items",
                columns: new[] { "ProductID", "ProductVariantId" });

            migrationBuilder.AddForeignKey(
                name: "FK_cart_Items_Product_Variants_ProductID_ProductVariantId",
                table: "cart_Items",
                columns: new[] { "ProductID", "ProductVariantId" },
                principalTable: "Product_Variants",
                principalColumns: new[] { "ProductID", "VariantId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_Items_Product_Variants_ProductID_ProductVariantId",
                table: "cart_Items");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Product_Variants_ProductID_VariantId",
                table: "Product_Variants");

            migrationBuilder.DropIndex(
                name: "IX_cart_Items_ProductID_ProductVariantId",
                table: "cart_Items");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Variants_ProductID",
                table: "Product_Variants",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_cart_Items_ProductID",
                table: "cart_Items",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_cart_Items_ProductVariantId",
                table: "cart_Items",
                column: "ProductVariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_cart_Items_Product_Variants_ProductVariantId",
                table: "cart_Items",
                column: "ProductVariantId",
                principalTable: "Product_Variants",
                principalColumn: "Id");
        }
    }
}
