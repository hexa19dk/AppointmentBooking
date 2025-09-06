using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgencyBookAppointments.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmentCounterTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppointmentCounters",
                columns: table => new
                {
                    DateKey = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentCounters", x => x.DateKey);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentCounters");
        }
    }
}
