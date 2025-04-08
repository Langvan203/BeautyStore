using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace my_cosmetic_store.Migrations
{
    /// <inheritdoc />
    public partial class fix_db_v7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Items_Product_Variants_VariantId",
                table: "Order_Items");

            migrationBuilder.DropIndex(
                name: "IX_Order_Items_ProductID",
                table: "Order_Items");

            migrationBuilder.DropIndex(
                name: "IX_Order_Items_VariantId",
                table: "Order_Items");

            migrationBuilder.RenameColumn(
                name: "VariantId",
                table: "Order_Items",
                newName: "ProductVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Items_ProductID_ProductVariantId",
                table: "Order_Items",
                columns: new[] { "ProductID", "ProductVariantId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Items_Product_Variants_ProductID_ProductVariantId",
                table: "Order_Items",
                columns: new[] { "ProductID", "ProductVariantId" },
                principalTable: "Product_Variants",
                principalColumns: new[] { "ProductID", "VariantId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Items_Product_Variants_ProductID_ProductVariantId",
                table: "Order_Items");

            migrationBuilder.DropIndex(
                name: "IX_Order_Items_ProductID_ProductVariantId",
                table: "Order_Items");

            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "Order_Items",
                newName: "VariantId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Items_ProductID",
                table: "Order_Items",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Items_VariantId",
                table: "Order_Items",
                column: "VariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Items_Product_Variants_VariantId",
                table: "Order_Items",
                column: "VariantId",
                principalTable: "Product_Variants",
                principalColumn: "Id");
        }
    }
}
