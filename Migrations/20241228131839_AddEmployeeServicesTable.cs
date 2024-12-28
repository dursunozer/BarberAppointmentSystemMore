using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberAppointmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeServicesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeServices_Employees_EmployeesEmployeeId",
                table: "EmployeeServices");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeServices_Services_ServicesServiceId",
                table: "EmployeeServices");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "ServicesServiceId",
                table: "EmployeeServices",
                newName: "ServiceId");

            migrationBuilder.RenameColumn(
                name: "EmployeesEmployeeId",
                table: "EmployeeServices",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeServices_ServicesServiceId",
                table: "EmployeeServices",
                newName: "IX_EmployeeServices_ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeServices_Employees_EmployeeId",
                table: "EmployeeServices",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeServices_Services_ServiceId",
                table: "EmployeeServices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeServices_Employees_EmployeeId",
                table: "EmployeeServices");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeServices_Services_ServiceId",
                table: "EmployeeServices");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "EmployeeServices",
                newName: "ServicesServiceId");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "EmployeeServices",
                newName: "EmployeesEmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeServices_ServiceId",
                table: "EmployeeServices",
                newName: "IX_EmployeeServices_ServicesServiceId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeServices_Employees_EmployeesEmployeeId",
                table: "EmployeeServices",
                column: "EmployeesEmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeServices_Services_ServicesServiceId",
                table: "EmployeeServices",
                column: "ServicesServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
