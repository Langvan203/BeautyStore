using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace my_cosmetic_store.Migrations
{
    /// <inheritdoc />
    public partial class fix_db_v12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_Items_ProductColor_ProductID_ProductColorId",
                table: "cart_Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Items_ProductColor_ProductID_ProductColorId",
                table: "Order_Items");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductColor_Color_ColorID",
                table: "ProductColor");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductColor_Products_ProductID",
                table: "ProductColor");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProductColor_ProductID_ColorID",
                table: "ProductColor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductColor",
                table: "ProductColor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Color",
                table: "Color");

            migrationBuilder.RenameTable(
                name: "ProductColor",
                newName: "Product_Colors");

            migrationBuilder.RenameTable(
                name: "Color",
                newName: "Colors");

            migrationBuilder.RenameIndex(
                name: "IX_ProductColor_ColorID",
                table: "Product_Colors",
                newName: "IX_Product_Colors_ColorID");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Product_Colors_ProductID_ColorID",
                table: "Product_Colors",
                columns: new[] { "ProductID", "ColorID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product_Colors",
                table: "Product_Colors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Colors",
                table: "Colors",
                column: "ColorID");

            migrationBuilder.AddForeignKey(
                name: "FK_cart_Items_Product_Colors_ProductID_ProductColorId",
                table: "cart_Items",
                columns: new[] { "ProductID", "ProductColorId" },
                principalTable: "Product_Colors",
                principalColumns: new[] { "ProductID", "ColorID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Items_Product_Colors_ProductID_ProductColorId",
                table: "Order_Items",
                columns: new[] { "ProductID", "ProductColorId" },
                principalTable: "Product_Colors",
                principalColumns: new[] { "ProductID", "ColorID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Colors_Colors_ColorID",
                table: "Product_Colors",
                column: "ColorID",
                principalTable: "Colors",
                principalColumn: "ColorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Colors_Products_ProductID",
                table: "Product_Colors",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_Items_Product_Colors_ProductID_ProductColorId",
                table: "cart_Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Items_Product_Colors_ProductID_ProductColorId",
                table: "Order_Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Colors_Colors_ColorID",
                table: "Product_Colors");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Colors_Products_ProductID",
                table: "Product_Colors");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Product_Colors_ProductID_ColorID",
                table: "Product_Colors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product_Colors",
                table: "Product_Colors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Colors",
                table: "Colors");

            migrationBuilder.RenameTable(
                name: "Product_Colors",
                newName: "ProductColor");

            migrationBuilder.RenameTable(
                name: "Colors",
                newName: "Color");

            migrationBuilder.RenameIndex(
                name: "IX_Product_Colors_ColorID",
                table: "ProductColor",
                newName: "IX_ProductColor_ColorID");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProductColor_ProductID_ColorID",
                table: "ProductColor",
                columns: new[] { "ProductID", "ColorID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductColor",
                table: "ProductColor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Color",
                table: "Color",
                column: "ColorID");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ProductColor_Color_ColorID",
                table: "ProductColor",
                column: "ColorID",
                principalTable: "Color",
                principalColumn: "ColorID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductColor_Products_ProductID",
                table: "ProductColor",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
