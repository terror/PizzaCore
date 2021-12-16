using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaCore.Migrations {
  public partial class OrderMethod : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.AlterColumn<string>(
          name: "PostalCode",
          table: "OrderModels",
          type: "nvarchar(max)",
          nullable: true,
          oldClrType: typeof(string),
          oldType: "nvarchar(max)");

      migrationBuilder.AlterColumn<string>(
          name: "City",
          table: "OrderModels",
          type: "nvarchar(50)",
          maxLength: 50,
          nullable: true,
          oldClrType: typeof(string),
          oldType: "nvarchar(50)",
          oldMaxLength: 50);

      migrationBuilder.AlterColumn<string>(
          name: "Address",
          table: "OrderModels",
          type: "nvarchar(max)",
          nullable: true,
          oldClrType: typeof(string),
          oldType: "nvarchar(max)");

      migrationBuilder.AddColumn<string>(
          name: "Method",
          table: "OrderModels",
          type: "nvarchar(max)",
          nullable: false,
          defaultValue: "");
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropColumn(
          name: "Method",
          table: "OrderModels");

      migrationBuilder.AlterColumn<string>(
          name: "PostalCode",
          table: "OrderModels",
          type: "nvarchar(max)",
          nullable: false,
          defaultValue: "",
          oldClrType: typeof(string),
          oldType: "nvarchar(max)",
          oldNullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "City",
          table: "OrderModels",
          type: "nvarchar(50)",
          maxLength: 50,
          nullable: false,
          defaultValue: "",
          oldClrType: typeof(string),
          oldType: "nvarchar(50)",
          oldMaxLength: 50,
          oldNullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "Address",
          table: "OrderModels",
          type: "nvarchar(max)",
          nullable: false,
          defaultValue: "",
          oldClrType: typeof(string),
          oldType: "nvarchar(max)",
          oldNullable: true);
    }
  }
}
