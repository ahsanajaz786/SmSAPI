using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiJwt.Migrations
{
    public partial class mymigratopn2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tenant_id",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "tenant_id",
                table: "User_Permissions",
                newName: "school_id");

            migrationBuilder.RenameColumn(
                name: "tenant_id",
                table: "User_Lead_Permissions",
                newName: "school_id");

            migrationBuilder.RenameColumn(
                name: "tenant_id",
                table: "Roles",
                newName: "school_id");

            migrationBuilder.RenameColumn(
                name: "tenant_id",
                table: "Role_Permissions",
                newName: "school_id");

            migrationBuilder.RenameColumn(
                name: "tenant_id",
                table: "Role_Lead_Permissions",
                newName: "school_id");

            migrationBuilder.RenameColumn(
                name: "tenant_id",
                table: "Meta_Fields",
                newName: "school_id");

            migrationBuilder.CreateTable(
                name: "todos",
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
                    title = table.Column<string>(nullable: true),
                    descrption = table.Column<string>(nullable: true),
                    date_Time = table.Column<DateTime>(nullable: false),
                    type = table.Column<int>(nullable: false),
                    score = table.Column<double>(nullable: false),
                    subject_Id = table.Column<int>(nullable: false),
                    teacher_Id = table.Column<int>(nullable: false),
                    class_Id = table.Column<int>(nullable: false),
                    section_Id = table.Column<int>(nullable: false),
                    year_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_todos", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "todos");

            migrationBuilder.RenameColumn(
                name: "school_id",
                table: "User_Permissions",
                newName: "tenant_id");

            migrationBuilder.RenameColumn(
                name: "school_id",
                table: "User_Lead_Permissions",
                newName: "tenant_id");

            migrationBuilder.RenameColumn(
                name: "school_id",
                table: "Roles",
                newName: "tenant_id");

            migrationBuilder.RenameColumn(
                name: "school_id",
                table: "Role_Permissions",
                newName: "tenant_id");

            migrationBuilder.RenameColumn(
                name: "school_id",
                table: "Role_Lead_Permissions",
                newName: "tenant_id");

            migrationBuilder.RenameColumn(
                name: "school_id",
                table: "Meta_Fields",
                newName: "tenant_id");

            migrationBuilder.AddColumn<int>(
                name: "tenant_id",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }
    }
}
