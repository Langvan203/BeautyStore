using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace my_cosmetic_store.Migrations
{
    /// <inheritdoc />
    public partial class fix_field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PriceOfVariant",
                table: "Product_Variants",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "StockOfVariant",
                table: "Product_Variants",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceOfVariant",
                table: "Product_Variants");

            migrationBuilder.DropColumn(
                name: "StockOfVariant",
                table: "Product_Variants");
        }
    }
}
