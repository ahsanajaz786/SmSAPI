using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiJwt.Migrations
{
    public partial class ten : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "year_Id",
                table: "todos");

            migrationBuilder.AlterColumn<string>(
                name: "teacher_Id",
                table: "subjectScores",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "student_Id",
                table: "subjectScores",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "year_Id",
                table: "todos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "teacher_Id",
                table: "subjectScores",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "student_Id",
                table: "subjectScores",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
