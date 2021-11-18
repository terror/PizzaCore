using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaCore.Migrations
{
    public partial class Addedimagecolumnformenutable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ItemImage",
                table: "MenuModel",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemImage",
                table: "MenuModel");
        }
    }
}
