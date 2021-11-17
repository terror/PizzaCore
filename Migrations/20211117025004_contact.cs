using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaCore.Migrations {
  public partial class contact : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.CreateTable(
          name: "ContactModel",
          columns: table => new {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            Email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
            Topic = table.Column<string>(type: "nvarchar(max)", nullable: false),
            Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
            Date = table.Column<DateTime>(type: "datetime2", nullable: false)
          },
          constraints: table => {
            table.PrimaryKey("PK_ContactModel", x => x.Id);
          });
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropTable(
          name: "ContactModel");
    }
  }
}
