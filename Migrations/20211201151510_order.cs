using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaCore.Migrations {
  public partial class order : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.CreateTable(
          name: "OrderModels",
          columns: table => new {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            Email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
            Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
            Payment = table.Column<string>(type: "nvarchar(max)", nullable: false),
            Date = table.Column<DateTime>(type: "datetime2", nullable: false)
          },
          constraints: table => {
            table.PrimaryKey("PK_OrderModels", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "CartItem",
          columns: table => new {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            ProductSizeId = table.Column<int>(type: "int", nullable: true),
            Quantity = table.Column<int>(type: "int", nullable: false),
            OrderModelId = table.Column<int>(type: "int", nullable: true)
          },
          constraints: table => {
            table.PrimaryKey("PK_CartItem", x => x.Id);
            table.ForeignKey(
                      name: "FK_CartItem_OrderModels_OrderModelId",
                      column: x => x.OrderModelId,
                      principalTable: "OrderModels",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_CartItem_ProductSizes_ProductSizeId",
                      column: x => x.ProductSizeId,
                      principalTable: "ProductSizes",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateIndex(
          name: "IX_CartItem_OrderModelId",
          table: "CartItem",
          column: "OrderModelId");

      migrationBuilder.CreateIndex(
          name: "IX_CartItem_ProductSizeId",
          table: "CartItem",
          column: "ProductSizeId");
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropTable(
          name: "CartItem");

      migrationBuilder.DropTable(
          name: "OrderModels");
    }
  }
}
