using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaCore.Migrations
{
  public partial class UserData : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {

      migrationBuilder.CreateTable(
          name: "UserDatas",
          columns: table => new
          {
            UserDataId = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            IdentityUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
            FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
            City = table.Column<string>(type: "nvarchar(max)", nullable: true),
            PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_UserDatas", x => x.UserDataId);
          });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "UserDatas");
    }
  }
}
