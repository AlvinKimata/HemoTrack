using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HemoTrack.Migrations
{
    public partial class AddAddressDoBToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DateOfBirth",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "AppointmentTime",
                table: "Appointment",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AppointmentDate",
                table: "Appointment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "User",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "AppointmentTime",
                table: "Appointment",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AppointmentDate",
                table: "Appointment",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
