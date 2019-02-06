using Microsoft.EntityFrameworkCore.Migrations;

namespace MySite.Migrations
{
    public partial class v11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_gropsAndProducts_groups_GroupId",
                table: "gropsAndProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_gropsAndProducts_products_ProductId",
                table: "gropsAndProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_groups_categories_CategoryId",
                table: "groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_products_ProductId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_ProductId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_groups_CategoryId",
                table: "groups");

            migrationBuilder.DropIndex(
                name: "IX_gropsAndProducts_GroupId",
                table: "gropsAndProducts");

            migrationBuilder.DropIndex(
                name: "IX_gropsAndProducts_ProductId",
                table: "gropsAndProducts");

            migrationBuilder.AddColumn<int>(
                name: "GroupsId",
                table: "gropsAndProducts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_gropsAndProducts_GroupsId",
                table: "gropsAndProducts",
                column: "GroupsId");

            migrationBuilder.AddForeignKey(
                name: "FK_gropsAndProducts_groups_GroupsId",
                table: "gropsAndProducts",
                column: "GroupsId",
                principalTable: "groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_gropsAndProducts_groups_GroupsId",
                table: "gropsAndProducts");

            migrationBuilder.DropIndex(
                name: "IX_gropsAndProducts_GroupsId",
                table: "gropsAndProducts");

            migrationBuilder.DropColumn(
                name: "GroupsId",
                table: "gropsAndProducts");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProductId",
                table: "Images",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_groups_CategoryId",
                table: "groups",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_gropsAndProducts_GroupId",
                table: "gropsAndProducts",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_gropsAndProducts_ProductId",
                table: "gropsAndProducts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_gropsAndProducts_groups_GroupId",
                table: "gropsAndProducts",
                column: "GroupId",
                principalTable: "groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_gropsAndProducts_products_ProductId",
                table: "gropsAndProducts",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_groups_categories_CategoryId",
                table: "groups",
                column: "CategoryId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_products_ProductId",
                table: "Images",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
