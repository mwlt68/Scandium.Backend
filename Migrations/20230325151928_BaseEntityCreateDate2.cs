using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scandium.Migrations
{
    public partial class BaseEntityCreateDate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 3, 25, 14, 43, 53, 607, DateTimeKind.Utc).AddTicks(5743));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 3, 25, 14, 43, 53, 607, DateTimeKind.Utc).AddTicks(5743),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "NOW()");
        }
    }
}
