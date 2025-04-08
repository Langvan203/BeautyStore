using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace my_cosmetic_store.Migrations
{
    /// <inheritdoc />
    public partial class fix_db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VariantId",
                table: "Order_Items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_Items_VariantId",
                table: "Order_Items",
                column: "VariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Items_Product_Variants_VariantId",
                table: "Order_Items",
                column: "VariantId",
                principalTable: "Product_Variants",
                principalColumn: "VariantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Items_Product_Variants_VariantId",
                table: "Order_Items");

            migrationBuilder.DropIndex(
                name: "IX_Order_Items_VariantId",
                table: "Order_Items");

            migrationBuilder.DropColumn(
                name: "VariantId",
                table: "Order_Items");
        }
    }
}
