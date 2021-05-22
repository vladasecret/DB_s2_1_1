using Microsoft.EntityFrameworkCore.Migrations;

namespace DB_s2_1_1.Migrations
{
    public partial class addRouteInTrain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RouteId",
                table: "Trains",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Trains_RouteId",
                table: "Trains",
                column: "RouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trains_Routes_RouteId",
                table: "Trains",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trains_Routes_RouteId",
                table: "Trains");

            migrationBuilder.DropIndex(
                name: "IX_Trains_RouteId",
                table: "Trains");

            migrationBuilder.AlterColumn<int>(
                name: "RouteId",
                table: "Trains",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
