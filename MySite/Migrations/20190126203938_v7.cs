using Microsoft.EntityFrameworkCore.Migrations;

namespace MySite.Migrations
{
    public partial class v7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_products_ProductsId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_ProductsId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ProductsId",
                table: "Images");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProductId",
                table: "Images",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_products_ProductId",
                table: "Images",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_products_ProductId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_ProductId",
                table: "Images");

            migrationBuilder.AddColumn<int>(
                name: "ProductsId",
                table: "Images",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProductsId",
                table: "Images",
                column: "ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_products_ProductsId",
                table: "Images",
                column: "ProductsId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
