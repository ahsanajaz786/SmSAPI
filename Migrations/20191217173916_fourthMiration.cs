using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiJwt.Migrations
{
    public partial class fourthMiration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mysubjectn_Id",
                table: "class_Subjects");

            migrationBuilder.RenameColumn(
                name: "class_subject_Id",
                table: "class_Subjects",
                newName: "subject_Id");

            migrationBuilder.RenameColumn(
                name: "class_id",
                table: "class_Subjects",
                newName: "class_section_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "subject_Id",
                table: "class_Subjects",
                newName: "class_subject_Id");

            migrationBuilder.RenameColumn(
                name: "class_section_Id",
                table: "class_Subjects",
                newName: "class_id");

            migrationBuilder.AddColumn<int>(
                name: "Mysubjectn_Id",
                table: "class_Subjects",
                nullable: false,
                defaultValue: 0);
        }
    }
}
