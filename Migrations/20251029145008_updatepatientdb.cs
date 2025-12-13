using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloodBankManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class updatepatientdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HospitalId",
                table: "PatientRecords",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientRecords_HospitalId",
                table: "PatientRecords",
                column: "HospitalId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientRecords_HospitalInformations_HospitalId",
                table: "PatientRecords",
                column: "HospitalId",
                principalTable: "HospitalInformations",
                principalColumn: "HospitalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientRecords_HospitalInformations_HospitalId",
                table: "PatientRecords");

            migrationBuilder.DropIndex(
                name: "IX_PatientRecords_HospitalId",
                table: "PatientRecords");

            migrationBuilder.DropColumn(
                name: "HospitalId",
                table: "PatientRecords");
        }
    }
}
