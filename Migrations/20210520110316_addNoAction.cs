using Microsoft.EntityFrameworkCore.Migrations;

namespace DB_s2_1_1.Migrations
{
    public partial class addNoAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StationRoads_Stations_FirstStationId",
                table: "StationRoads");

            migrationBuilder.AddForeignKey(
                name: "FK_StationRoads_Stations_FirstStationId",
                table: "StationRoads",
                column: "FirstStationId",
                principalTable: "Stations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StationRoads_Stations_FirstStationId",
                table: "StationRoads");

            migrationBuilder.AddForeignKey(
                name: "FK_StationRoads_Stations_FirstStationId",
                table: "StationRoads",
                column: "FirstStationId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
