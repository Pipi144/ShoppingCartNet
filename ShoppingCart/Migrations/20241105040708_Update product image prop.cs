using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingCart.Migrations
{
    /// <inheritdoc />
    public partial class Updateproductimageprop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Product",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Product",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");
        }
    }
}
