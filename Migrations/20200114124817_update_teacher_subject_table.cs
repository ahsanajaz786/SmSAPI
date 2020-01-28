using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiJwt.Migrations
{
    public partial class update_teacher_subject_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "teacher_Id",
                table: "teacher_Subjects",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "teacher_Id",
                table: "teacher_Subjects",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
