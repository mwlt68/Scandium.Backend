using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scandium.Migrations
{
    public partial class UpdateMessageContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Messages");

            migrationBuilder.AddColumn<byte[]>(
                name: "Contents",
                table: "Messages",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contents",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Messages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
