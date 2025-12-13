using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloodBankManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class newfieldsadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "DonorRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "DonorRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DonorRecords_PatientId",
                table: "DonorRecords",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_DonorRecords_PatientRecords_PatientId",
                table: "DonorRecords",
                column: "PatientId",
                principalTable: "PatientRecords",
                principalColumn: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonorRecords_PatientRecords_PatientId",
                table: "DonorRecords");

            migrationBuilder.DropIndex(
                name: "IX_DonorRecords_PatientId",
                table: "DonorRecords");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "DonorRecords");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "DonorRecords");
        }
    }
}
