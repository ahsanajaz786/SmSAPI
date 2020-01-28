using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiJwt.Migrations
{
    public partial class gsyfw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "end_Date",
                table: "school_Years");

            migrationBuilder.DropColumn(
                name: "start_Date",
                table: "school_Years");

            migrationBuilder.DropColumn(
                name: "year",
                table: "school_Years");

            migrationBuilder.AddColumn<int>(
                name: "YearID",
                table: "school_Years",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "year",
                table: "global_School_Years",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YearID",
                table: "school_Years");

            migrationBuilder.AddColumn<DateTime>(
                name: "end_Date",
                table: "school_Years",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "start_Date",
                table: "school_Years",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "year",
                table: "school_Years",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "year",
                table: "global_School_Years",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
