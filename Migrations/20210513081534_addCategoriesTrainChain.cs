using Microsoft.EntityFrameworkCore.Migrations;

namespace DB_s2_1_1.Migrations
{
    public partial class addCategoriesTrainChain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Trains");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Trains",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
