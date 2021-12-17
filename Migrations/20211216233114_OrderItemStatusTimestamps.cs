using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaCore.Migrations
{
    public partial class OrderItemStatusTimestamps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "OrderItems",
                newName: "Status");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PreparingTimeStamp",
                table: "OrderItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReadyTimeStamp",
                table: "OrderItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "PreparingTimeStamp",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ReadyTimeStamp",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "OrderItems",
                newName: "ProductId");
        }
    }
}
