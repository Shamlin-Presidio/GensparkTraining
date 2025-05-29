using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstAPI.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AppointmnetDateTime",
                table: "Appointments",
                newName: "AppointmentDateTime");

            migrationBuilder.RenameColumn(
                name: "AppointmnetNumber",
                table: "Appointments",
                newName: "AppointmentNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AppointmentDateTime",
                table: "Appointments",
                newName: "AppointmnetDateTime");

            migrationBuilder.RenameColumn(
                name: "AppointmentNumber",
                table: "Appointments",
                newName: "AppointmnetNumber");
        }
    }
}
