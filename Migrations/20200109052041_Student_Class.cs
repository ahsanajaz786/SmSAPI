using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiJwt.Migrations
{
    public partial class Student_Class : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "student_Classes",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    school_id = table.Column<int>(nullable: false),
                    is_deleted = table.Column<bool>(nullable: false),
                    created_by = table.Column<string>(nullable: true),
                    year_id = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<string>(nullable: true),
                    modified_date = table.Column<DateTime>(nullable: false),
                    student_Id = table.Column<string>(nullable: true),
                    class_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_Classes", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "student_Classes");
        }
    }
}
