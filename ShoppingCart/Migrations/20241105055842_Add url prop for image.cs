using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingCart.Migrations
{
    /// <inheritdoc />
    public partial class Addurlpropforimage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ProductImage",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ProductImage");
        }
    }
}
