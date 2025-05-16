using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace my_cosmetic_store.Migrations
{
    /// <inheritdoc />
    public partial class fix_db_v11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColorName",
                table: "Color",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorName",
                table: "Color");
        }
    }
}
