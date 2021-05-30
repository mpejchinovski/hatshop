using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hatshop.Migrations
{
    public partial class PicturePath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Hat");


            migrationBuilder.AddColumn<string>(
                name: "PicturePath",
                table: "Hat",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PicturePath",
                table: "Hat");

            migrationBuilder.AddColumn<byte[]>(
                name: "Picture",
                table: "Hat",
                type: "longblob",
                nullable: true);
        }
    }
}
