using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiJwt.Migrations
{
    public partial class updatedTodos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "descrption",
                table: "todos",
                newName: "fileUpload");

            migrationBuilder.RenameColumn(
                name: "lavel",
                table: "classes",
                newName: "level");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "todos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "todos");

            migrationBuilder.RenameColumn(
                name: "fileUpload",
                table: "todos",
                newName: "descrption");

            migrationBuilder.RenameColumn(
                name: "level",
                table: "classes",
                newName: "lavel");
        }
    }
}
