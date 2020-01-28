using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiJwt.Migrations
{
    public partial class change_classSection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "section_Name",
                table: "class_Sections");

            migrationBuilder.AddColumn<int>(
                name: "section_id",
                table: "class_Sections",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "section_id",
                table: "class_Sections");

            migrationBuilder.AddColumn<string>(
                name: "section_Name",
                table: "class_Sections",
                nullable: true);
        }
    }
}
