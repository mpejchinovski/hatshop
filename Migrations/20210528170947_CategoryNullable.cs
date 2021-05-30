using Microsoft.EntityFrameworkCore.Migrations;

namespace hatshop.Migrations
{
    public partial class CategoryNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hats_Categories_CategoryId",
                table: "Hats");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Hats",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Hats_Categories_CategoryId",
                table: "Hats",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hats_Categories_CategoryId",
                table: "Hats");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Hats",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Hats_Categories_CategoryId",
                table: "Hats",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
