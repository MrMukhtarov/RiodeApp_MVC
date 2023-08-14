using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiodeApp.Migrations
{
    public partial class UpdateSliderTableAddISrightColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Isright",
                table: "Sliders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Isright",
                table: "Sliders");
        }
    }
}
