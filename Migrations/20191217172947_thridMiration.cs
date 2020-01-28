using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiJwt.Migrations
{
    public partial class thridMiration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "class_Sections",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    school_id = table.Column<int>(nullable: false),
                    is_deleted = table.Column<bool>(nullable: false),
                    created_by = table.Column<string>(nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<string>(nullable: true),
                    modified_date = table.Column<DateTime>(nullable: false),
                    class_ID = table.Column<int>(nullable: false),
                    section_Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class_Sections", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "class_Subjects",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    school_id = table.Column<int>(nullable: false),
                    is_deleted = table.Column<bool>(nullable: false),
                    created_by = table.Column<string>(nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<string>(nullable: true),
                    modified_date = table.Column<DateTime>(nullable: false),
                    class_subject_Id = table.Column<int>(nullable: false),
                    class_id = table.Column<int>(nullable: false),
                    Mysubjectn_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class_Subjects", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "classes",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    school_id = table.Column<int>(nullable: false),
                    is_deleted = table.Column<bool>(nullable: false),
                    created_by = table.Column<string>(nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<string>(nullable: true),
                    modified_date = table.Column<DateTime>(nullable: false),
                    lavel = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "subjects",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    school_id = table.Column<int>(nullable: false),
                    is_deleted = table.Column<bool>(nullable: false),
                    created_by = table.Column<string>(nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<string>(nullable: true),
                    modified_date = table.Column<DateTime>(nullable: false),
                    subject_Name = table.Column<string>(nullable: true),
                    title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subjects", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "class_Sections");

            migrationBuilder.DropTable(
                name: "class_Subjects");

            migrationBuilder.DropTable(
                name: "classes");

            migrationBuilder.DropTable(
                name: "subjects");
        }
    }
}
