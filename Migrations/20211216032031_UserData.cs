using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaCore.Migrations {
  public partial class UserData : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.CreateTable(
          name: "UserDatas",
          columns: table => new {
            UserDataId = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            IdentityUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
            FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
            City = table.Column<string>(type: "nvarchar(max)", nullable: true),
            PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
          },
          constraints: table => {
            table.PrimaryKey("PK_UserDatas", x => x.UserDataId);
          });

      migrationBuilder.DropTable(
          name: "OrderModels");

      migrationBuilder.CreateTable(
          name: "OrderModels",
          columns: table => new {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            Email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
            Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
            Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
            Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
            City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
            Payment = table.Column<string>(type: "nvarchar(max)", nullable: false),
            Date = table.Column<DateTime>(type: "datetime2", nullable: false),
            SubTotal = table.Column<double>(type: "float", nullable: false),
            ShippingCost = table.Column<double>(type: "float", nullable: false),
            Taxes = table.Column<double>(type: "float", nullable: false),
            UserDataId = table.Column<int>(type: "int", nullable: true)
          },
          constraints: table => {
            table.PrimaryKey("PK_OrderModels", x => x.Id);
            table.ForeignKey(
                      name: "FK_OrderModels_UserDatas_UserDataId",
                      column: x => x.UserDataId,
                      principalTable: "UserDatas",
                      principalColumn: "UserDataId",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateIndex(
          name: "IX_OrderModels_UserDataId",
          table: "OrderModels",
          column: "UserDataId");
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropTable(
          name: "OrderModels");

      migrationBuilder.DropTable(
          name: "UserDatas");
    }
  }
}
