using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiJwt.Migrations
{
    public partial class advisorupdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "advisor",
                table: "class_Subjects");

            migrationBuilder.AddColumn<string>(
                name: "advisor",
                table: "class_Sections",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "advisor",
                table: "class_Sections");

            migrationBuilder.AddColumn<string>(
                name: "advisor",
                table: "class_Subjects",
                nullable: true);
        }
    }
}
