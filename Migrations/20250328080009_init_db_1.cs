using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace my_cosmetic_store.Migrations
{
    /// <inheritdoc />
    public partial class init_db_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_Items_Product_Variants_ProductVariantID",
                table: "cart_Items");

            migrationBuilder.DropIndex(
                name: "IX_Product_Variants_VariantId",
                table: "Product_Variants");

            migrationBuilder.DropIndex(
                name: "IX_cart_Items_ProductVariantID",
                table: "cart_Items");

            migrationBuilder.DropColumn(
                name: "ProductVariantID",
                table: "cart_Items");

            migrationBuilder.AddColumn<int>(
                name: "VariantId",
                table: "cart_Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Product_Variants_VariantId",
                table: "Product_Variants",
                column: "VariantId");

            migrationBuilder.CreateIndex(
                name: "IX_cart_Items_VariantId",
                table: "cart_Items",
                column: "VariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_cart_Items_Product_Variants_VariantId",
                table: "cart_Items",
                column: "VariantId",
                principalTable: "Product_Variants",
                principalColumn: "VariantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_Items_Product_Variants_VariantId",
                table: "cart_Items");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Product_Variants_VariantId",
                table: "Product_Variants");

            migrationBuilder.DropIndex(
                name: "IX_cart_Items_VariantId",
                table: "cart_Items");

            migrationBuilder.DropColumn(
                name: "VariantId",
                table: "cart_Items");

            migrationBuilder.AddColumn<int>(
                name: "ProductVariantID",
                table: "cart_Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Product_Variants_VariantId",
                table: "Product_Variants",
                column: "VariantId");

            migrationBuilder.CreateIndex(
                name: "IX_cart_Items_ProductVariantID",
                table: "cart_Items",
                column: "ProductVariantID");

            migrationBuilder.AddForeignKey(
                name: "FK_cart_Items_Product_Variants_ProductVariantID",
                table: "cart_Items",
                column: "ProductVariantID",
                principalTable: "Product_Variants",
                principalColumn: "Id");
        }
    }
}
