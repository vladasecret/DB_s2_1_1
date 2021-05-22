using Microsoft.EntityFrameworkCore.Migrations;

namespace DB_s2_1_1.Migrations
{
    public partial class cascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StationRoads_Stations_SecondStationId",
                table: "StationRoads");

            migrationBuilder.AddForeignKey(
                name: "FK_StationRoads_Stations_SecondStationId",
                table: "StationRoads",
                column: "SecondStationId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StationRoads_Stations_SecondStationId",
                table: "StationRoads");

            migrationBuilder.AddForeignKey(
                name: "FK_StationRoads_Stations_SecondStationId",
                table: "StationRoads",
                column: "SecondStationId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
