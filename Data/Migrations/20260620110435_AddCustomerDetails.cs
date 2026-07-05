using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AadharCardNumber",
                table: "CustomerDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DrivingLicenseNumber",
                table: "CustomerDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Others",
                table: "CustomerDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PanCardNumber",
                table: "CustomerDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassportNumber",
                table: "CustomerDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AadharCardNumber",
                table: "CustomerDetails");

            migrationBuilder.DropColumn(
                name: "DrivingLicenseNumber",
                table: "CustomerDetails");

            migrationBuilder.DropColumn(
                name: "Others",
                table: "CustomerDetails");

            migrationBuilder.DropColumn(
                name: "PanCardNumber",
                table: "CustomerDetails");

            migrationBuilder.DropColumn(
                name: "PassportNumber",
                table: "CustomerDetails");
        }
    }
}
