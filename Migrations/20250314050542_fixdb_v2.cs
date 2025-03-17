using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace my_cosmetic_store.Migrations
{
    /// <inheritdoc />
    public partial class fixdb_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_cart_Items_CartID",
                table: "cart_Items",
                column: "CartID");

            migrationBuilder.AddForeignKey(
                name: "FK_cart_Items_Carts_CartID",
                table: "cart_Items",
                column: "CartID",
                principalTable: "Carts",
                principalColumn: "CartID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_Items_Carts_CartID",
                table: "cart_Items");

            migrationBuilder.DropIndex(
                name: "IX_cart_Items_CartID",
                table: "cart_Items");
        }
    }
}
