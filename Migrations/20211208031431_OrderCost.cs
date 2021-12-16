using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaCore.Migrations {
  public partial class OrderCost : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.AddColumn<double>(
          name: "ShippingCost",
          table: "OrderModels",
          type: "float",
          nullable: false,
          defaultValue: 0.0);

      migrationBuilder.AddColumn<double>(
          name: "SubTotal",
          table: "OrderModels",
          type: "float",
          nullable: false,
          defaultValue: 0.0);

      migrationBuilder.AddColumn<double>(
          name: "Taxes",
          table: "OrderModels",
          type: "float",
          nullable: false,
          defaultValue: 0.0);
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropColumn(
          name: "ShippingCost",
          table: "OrderModels");

      migrationBuilder.DropColumn(
          name: "SubTotal",
          table: "OrderModels");

      migrationBuilder.RenameColumn(
          name: "Taxes",
          table: "OrderModels",
          newName: "ItemPrice");
    }
  }
}
