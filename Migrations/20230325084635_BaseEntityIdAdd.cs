using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scandium.Migrations
{
    public partial class BaseEntityIdAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Users");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:uuid-ossp", ",,");
        }
    }
}
