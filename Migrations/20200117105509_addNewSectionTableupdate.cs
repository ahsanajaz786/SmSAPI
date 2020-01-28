using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiJwt.Migrations
{
    public partial class addNewSectionTableupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "section",
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
                    sectionName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_section", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "section");
        }
    }
}
