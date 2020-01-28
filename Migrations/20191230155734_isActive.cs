using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiJwt.Migrations
{
    public partial class isActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "filrName",
                table: "todos",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "school_Years",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "filrName",
                table: "todos");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "school_Years");
        }
    }
}
