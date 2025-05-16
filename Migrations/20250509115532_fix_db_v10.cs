using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace my_cosmetic_store.Migrations
{
    /// <inheritdoc />
    public partial class fix_db_v10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductColor_ProductID",
                table: "ProductColor");

            migrationBuilder.AddColumn<int>(
                name: "ProductColorId",
                table: "Order_Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductColorId",
                table: "cart_Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProductColor_ProductID_ColorID",
                table: "ProductColor",
                columns: new[] { "ProductID", "ColorID" });

            migrationBuilder.CreateIndex(
                name: "IX_Order_Items_ProductID_ProductColorId",
                table: "Order_Items",
                columns: new[] { "ProductID", "ProductColorId" });

            migrationBuilder.CreateIndex(
                name: "IX_cart_Items_ProductID_ProductColorId",
                table: "cart_Items",
                columns: new[] { "ProductID", "ProductColorId" });

            migrationBuilder.AddForeignKey(
                name: "FK_cart_Items_ProductColor_ProductID_ProductColorId",
                table: "cart_Items",
                columns: new[] { "ProductID", "ProductColorId" },
                principalTable: "ProductColor",
                principalColumns: new[] { "ProductID", "ColorID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Items_ProductColor_ProductID_ProductColorId",
                table: "Order_Items",
                columns: new[] { "ProductID", "ProductColorId" },
                principalTable: "ProductColor",
                principalColumns: new[] { "ProductID", "ColorID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_Items_ProductColor_ProductID_ProductColorId",
                table: "cart_Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Items_ProductColor_ProductID_ProductColorId",
                table: "Order_Items");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProductColor_ProductID_ColorID",
                table: "ProductColor");

            migrationBuilder.DropIndex(
                name: "IX_Order_Items_ProductID_ProductColorId",
                table: "Order_Items");

            migrationBuilder.DropIndex(
                name: "IX_cart_Items_ProductID_ProductColorId",
                table: "cart_Items");

            migrationBuilder.DropColumn(
                name: "ProductColorId",
                table: "Order_Items");

            migrationBuilder.DropColumn(
                name: "ProductColorId",
                table: "cart_Items");

            migrationBuilder.CreateIndex(
                name: "IX_ProductColor_ProductID",
                table: "ProductColor",
                column: "ProductID");
        }
    }
}
