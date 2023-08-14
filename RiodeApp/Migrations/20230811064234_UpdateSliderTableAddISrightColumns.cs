using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiodeApp.Migrations
{
    public partial class UpdateSliderTableAddISrightColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Isright",
                table: "Sliders",
                newName: "iSright");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "iSright",
                table: "Sliders",
                newName: "Isright");
        }
    }
}
