using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusTrackingDL.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintToTrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trips_DriverId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "DropoffLocation",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "PickupLocation",
                table: "Trips");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "TripTime",
                table: "Trips",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualDepartureTime",
                table: "Trips",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DriverArrivedAt",
                table: "Trips",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TripDay",
                table: "Trips",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Trips_DriverId_TripDay_TripTime",
                table: "Trips",
                columns: new[] { "DriverId", "TripDay", "TripTime" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trips_DriverId_TripDay_TripTime",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "ActualDepartureTime",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "DriverArrivedAt",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "TripDay",
                table: "Trips");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TripTime",
                table: "Trips",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AddColumn<string>(
                name: "DropoffLocation",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PickupLocation",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_DriverId",
                table: "Trips",
                column: "DriverId");
        }
    }
}
