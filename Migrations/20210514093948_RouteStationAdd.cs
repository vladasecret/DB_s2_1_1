using Microsoft.EntityFrameworkCore.Migrations;

namespace DB_s2_1_1.Migrations
{
    public partial class RouteStationAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Stations_StationId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Trains_Routes_RouteId",
                table: "Trains");

            migrationBuilder.DropForeignKey(
                name: "FK_TransitRoutes_Routes_RouteId",
                table: "TransitRoutes");

            migrationBuilder.DropIndex(
                name: "IX_TransitRoutes_RouteId",
                table: "TransitRoutes");

            migrationBuilder.DropIndex(
                name: "IX_Trains_RouteId",
                table: "Trains");

            migrationBuilder.DropIndex(
                name: "IX_Routes_RouteId_StationId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_StationId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "RouteId",
                table: "TransitRoutes");

            migrationBuilder.DropColumn(
                name: "RouteId",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "RouteId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "StationId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "StationOrder",
                table: "Routes");

            migrationBuilder.CreateTable(
                name: "RouteStation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    StationId = table.Column<int>(type: "int", nullable: false),
                    StationOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteStation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteStation_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RouteStation_Stations_StationId",
                        column: x => x.StationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RouteStation_RouteId_StationId",
                table: "RouteStation",
                columns: new[] { "RouteId", "StationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RouteStation_StationId",
                table: "RouteStation",
                column: "StationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RouteStation");

            migrationBuilder.AddColumn<int>(
                name: "RouteId",
                table: "TransitRoutes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RouteId",
                table: "Trains",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RouteId",
                table: "Routes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StationId",
                table: "Routes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StationOrder",
                table: "Routes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TransitRoutes_RouteId",
                table: "TransitRoutes",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Trains_RouteId",
                table: "Trains",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_RouteId_StationId",
                table: "Routes",
                columns: new[] { "RouteId", "StationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_StationId",
                table: "Routes",
                column: "StationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Stations_StationId",
                table: "Routes",
                column: "StationId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trains_Routes_RouteId",
                table: "Trains",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransitRoutes_Routes_RouteId",
                table: "TransitRoutes",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
