using Microsoft.EntityFrameworkCore.Migrations;

namespace DB_s2_1_1.Migrations
{
    public partial class addRestrinct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StationRoads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstStationId = table.Column<int>(type: "int", nullable: false),
                    SecondStationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationRoads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StationRoads_Stations_FirstStationId",
                        column: x => x.FirstStationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StationRoads_Stations_SecondStationId",
                        column: x => x.SecondStationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StationRoads_FirstStationId_SecondStationId",
                table: "StationRoads",
                columns: new[] { "FirstStationId", "SecondStationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StationRoads_SecondStationId",
                table: "StationRoads",
                column: "SecondStationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StationRoads");
        }
    }
}
