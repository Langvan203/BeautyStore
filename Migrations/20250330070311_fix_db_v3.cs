using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace my_cosmetic_store.Migrations
{
    /// <inheritdoc />
    public partial class fix_db_v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_Items_Product_Variants_VariantId",
                table: "cart_Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Items_Product_Variants_VariantId",
                table: "Order_Items");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Product_Variants_VariantId",
                table: "Product_Variants");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Variants_VariantId",
                table: "Product_Variants",
                column: "VariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_cart_Items_Product_Variants_VariantId",
                table: "cart_Items",
                column: "VariantId",
                principalTable: "Product_Variants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Items_Product_Variants_VariantId",
                table: "Order_Items",
                column: "VariantId",
                principalTable: "Product_Variants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_Items_Product_Variants_VariantId",
                table: "cart_Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Items_Product_Variants_VariantId",
                table: "Order_Items");

            migrationBuilder.DropIndex(
                name: "IX_Product_Variants_VariantId",
                table: "Product_Variants");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Product_Variants_VariantId",
                table: "Product_Variants",
                column: "VariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_cart_Items_Product_Variants_VariantId",
                table: "cart_Items",
                column: "VariantId",
                principalTable: "Product_Variants",
                principalColumn: "VariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Items_Product_Variants_VariantId",
                table: "Order_Items",
                column: "VariantId",
                principalTable: "Product_Variants",
                principalColumn: "VariantId");
        }
    }
}
