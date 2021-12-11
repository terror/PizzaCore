using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaCore.Migrations
{
    public partial class UserData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserDataId",
                table: "OrderModels",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserDatas",
                columns: table => new
                {
                    UserDataId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDatas", x => x.UserDataId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderModels_UserDataId",
                table: "OrderModels",
                column: "UserDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderModels_UserDatas_UserDataId",
                table: "OrderModels",
                column: "UserDataId",
                principalTable: "UserDatas",
                principalColumn: "UserDataId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderModels_UserDatas_UserDataId",
                table: "OrderModels");

            migrationBuilder.DropTable(
                name: "UserDatas");

            migrationBuilder.DropIndex(
                name: "IX_OrderModels_UserDataId",
                table: "OrderModels");

            migrationBuilder.DropColumn(
                name: "UserDataId",
                table: "OrderModels");
        }
    }
}
