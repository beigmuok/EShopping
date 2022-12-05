using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShopping.Migrations
{
    public partial class AddProductQuantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quanity",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quanity",
                table: "Product");
        }
    }
}
