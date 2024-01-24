using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HemoTrack.Migrations
{
    public partial class RemoveIdentityFromAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "Administrator");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Administrator");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Administrator");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Administrator");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "Administrator");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "Administrator");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "Administrator");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "Administrator");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Administrator");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Administrator");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "Administrator");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "Administrator");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Administrator");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Administrator");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "Administrator",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Administrator",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Administrator",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Administrator",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "Administrator",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "Administrator",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "Administrator",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "Administrator",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Administrator",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Administrator",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "Administrator",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "Administrator",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Administrator",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Administrator",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
