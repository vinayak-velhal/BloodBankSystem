using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloodBankManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class adddoctorrelfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "DonorRecords",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DonorRecords_DoctorId",
                table: "DonorRecords",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_DonorRecords_DoctorInformations_DoctorId",
                table: "DonorRecords",
                column: "DoctorId",
                principalTable: "DoctorInformations",
                principalColumn: "DoctorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonorRecords_DoctorInformations_DoctorId",
                table: "DonorRecords");

            migrationBuilder.DropIndex(
                name: "IX_DonorRecords_DoctorId",
                table: "DonorRecords");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "DonorRecords");
        }
    }
}
