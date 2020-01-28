using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiJwt.Migrations
{
    public partial class @event : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "year",
                table: "global_School_Years",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "classEvents",
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
                    clasid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classEvents", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "eventAttachments",
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
                    event_id = table.Column<int>(nullable: false),
                    file_name = table.Column<string>(nullable: true),
                    file_path = table.Column<string>(nullable: true),
                    file_type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eventAttachments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "events",
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
                    title = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    location = table.Column<string>(nullable: true),
                    teacher_id = table.Column<string>(nullable: true),
                    startDateTime = table.Column<DateTime>(nullable: false),
                    endDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "classEvents");

            migrationBuilder.DropTable(
                name: "eventAttachments");

            migrationBuilder.DropTable(
                name: "events");

            migrationBuilder.AlterColumn<int>(
                name: "year",
                table: "global_School_Years",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
