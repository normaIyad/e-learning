using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Course.DAL.Migrations
{
    /// <inheritdoc />
    public partial class codeRest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordRestCode",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordRestCodeExpiration",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordRestCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordRestCodeExpiration",
                table: "Users");
        }
    }
}
