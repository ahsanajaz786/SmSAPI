using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiJwt.Migrations
{
    public partial class student_tabl1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GuardiaID",
                table: "students",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GuardiaID",
                table: "students",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
