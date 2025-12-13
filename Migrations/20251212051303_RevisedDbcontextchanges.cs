using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloodBankManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class RevisedDbcontextchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BloodInventories_DonorRecords_DonorId",
                table: "BloodInventories");

            migrationBuilder.CreateIndex(
                name: "IX_Consents_DonorId",
                table: "Consents",
                column: "DonorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BloodInventories_DonorRecords_DonorId",
                table: "BloodInventories",
                column: "DonorId",
                principalTable: "DonorRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Consents_DonorRecords_DonorId",
                table: "Consents",
                column: "DonorId",
                principalTable: "DonorRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientRecords_Billings_BillId",
                table: "PatientRecords",
                column: "BillId",
                principalTable: "Billings",
                principalColumn: "BillId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BloodInventories_DonorRecords_DonorId",
                table: "BloodInventories");

            migrationBuilder.DropForeignKey(
                name: "FK_Consents_DonorRecords_DonorId",
                table: "Consents");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientRecords_Billings_BillId",
                table: "PatientRecords");

            migrationBuilder.DropIndex(
                name: "IX_Consents_DonorId",
                table: "Consents");

            migrationBuilder.AddForeignKey(
                name: "FK_BloodInventories_DonorRecords_DonorId",
                table: "BloodInventories",
                column: "DonorId",
                principalTable: "DonorRecords",
                principalColumn: "Id");
        }
    }
}
