using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DB_s2_1_1.Migrations
{
    public partial class addWaitingsTimetableChain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Waitings_Trains_TrainId",
                table: "Waitings");

            migrationBuilder.DropIndex(
                name: "IX_Waitings_TrainId",
                table: "Waitings");

            migrationBuilder.DropColumn(
                name: "ArrivalTime",
                table: "Waitings");

            migrationBuilder.DropColumn(
                name: "TrainDirection",
                table: "Waitings");

            migrationBuilder.RenameColumn(
                name: "TrainId",
                table: "Waitings",
                newName: "TimetableId");

            migrationBuilder.CreateIndex(
                name: "IX_Waitings_TimetableId",
                table: "Waitings",
                column: "TimetableId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Waitings_Timetables_TimetableId",
                table: "Waitings",
                column: "TimetableId",
                principalTable: "Timetables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Waitings_Timetables_TimetableId",
                table: "Waitings");

            migrationBuilder.DropIndex(
                name: "IX_Waitings_TimetableId",
                table: "Waitings");

            migrationBuilder.RenameColumn(
                name: "TimetableId",
                table: "Waitings",
                newName: "TrainId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivalTime",
                table: "Waitings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "TrainDirection",
                table: "Waitings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Waitings_TrainId",
                table: "Waitings",
                column: "TrainId");

            migrationBuilder.AddForeignKey(
                name: "FK_Waitings_Trains_TrainId",
                table: "Waitings",
                column: "TrainId",
                principalTable: "Trains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
