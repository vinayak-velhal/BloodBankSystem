using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloodBankManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Consents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonorId = table.Column<int>(type: "int", nullable: false),
                    ConsentGiven = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SignatureHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DoctorInformations",
                columns: table => new
                {
                    DoctorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorName = table.Column<string>(type: "varchar(50)", nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DoctorAddress = table.Column<string>(type: "varchar(100)", nullable: false),
                    Qualification = table.Column<string>(type: "varchar(50)", nullable: false),
                    EmailId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorInformations", x => x.DoctorId);
                });

            migrationBuilder.CreateTable(
                name: "DonorRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "varchar(50)", nullable: false),
                    RelativeName = table.Column<string>(type: "varchar(50)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "varchar(10)", nullable: false),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DonationType = table.Column<string>(type: "varchar(20)", nullable: false),
                    BloodGroup = table.Column<string>(type: "varchar(50)", nullable: false),
                    DonationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastDonationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DoctorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HospitalName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AteRecently = table.Column<bool>(type: "bit", nullable: false),
                    FillingWell = table.Column<bool>(type: "bit", nullable: false),
                    HadFever = table.Column<bool>(type: "bit", nullable: false),
                    UnderTreatment = table.Column<bool>(type: "bit", nullable: false),
                    RecentSurgery = table.Column<bool>(type: "bit", nullable: false),
                    UsedAntibiotics = table.Column<bool>(type: "bit", nullable: false),
                    BloodTransfusion = table.Column<bool>(type: "bit", nullable: false),
                    Alcohol24Hours = table.Column<bool>(type: "bit", nullable: false),
                    RiskeyBehavior = table.Column<bool>(type: "bit", nullable: false),
                    HasTattoOrPiercing = table.Column<bool>(type: "bit", nullable: false),
                    TestedHIVPositive = table.Column<bool>(type: "bit", nullable: false),
                    HistoryOfJaundice = table.Column<bool>(type: "bit", nullable: false),
                    PrevRejectedForDonation = table.Column<bool>(type: "bit", nullable: false),
                    IsPregnant = table.Column<bool>(type: "bit", nullable: true),
                    IsBreastFeeding = table.Column<bool>(type: "bit", nullable: true),
                    RecentDeliveryOrAbortion = table.Column<bool>(type: "bit", nullable: true),
                    IsMenustruating = table.Column<bool>(type: "bit", nullable: true),
                    weight = table.Column<int>(type: "int", nullable: false),
                    Temperature = table.Column<float>(type: "real", nullable: false),
                    BloodPressure = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hemoglobin = table.Column<float>(type: "real", nullable: false),
                    FitToDonate = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonorRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeInformations",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeName = table.Column<string>(type: "varchar(50)", nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    EmployeeAddress = table.Column<string>(type: "varchar(100)", nullable: false),
                    Qualification = table.Column<string>(type: "varchar(50)", nullable: false),
                    EmailId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeInformations", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "HospitalInformations",
                columns: table => new
                {
                    HospitalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HospitalName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HospitalAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoctorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HospitalContact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonContact = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HospitalInformations", x => x.HospitalId);
                });

            migrationBuilder.CreateTable(
                name: "PatientRecords",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientName = table.Column<string>(type: "varchar(50)", nullable: false),
                    PatientAddress = table.Column<string>(type: "varchar(100)", nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DiseaseName = table.Column<string>(type: "varchar(50)", nullable: false),
                    BloodGroup = table.Column<string>(type: "varchar(50)", nullable: false),
                    HospitalName = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientRecords", x => x.PatientId);
                });

            migrationBuilder.CreateTable(
                name: "BloodInventories",
                columns: table => new
                {
                    InventoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BloodGroup = table.Column<string>(type: "varchar(50)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CollectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DonorId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodInventories", x => x.InventoryId);
                    table.ForeignKey(
                        name: "FK_BloodInventories_DonorRecords_DonorId",
                        column: x => x.DonorId,
                        principalTable: "DonorRecords",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BloodRequests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    PatientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BloodGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InventoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodRequests", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_BloodRequests_BloodInventories_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "BloodInventories",
                        principalColumn: "InventoryId");
                    table.ForeignKey(
                        name: "FK_BloodRequests_PatientRecords_PatientId",
                        column: x => x.PatientId,
                        principalTable: "PatientRecords",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BloodInventories_DonorId",
                table: "BloodInventories",
                column: "DonorId");

            migrationBuilder.CreateIndex(
                name: "IX_BloodRequests_InventoryId",
                table: "BloodRequests",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BloodRequests_PatientId",
                table: "BloodRequests",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BloodRequests");

            migrationBuilder.DropTable(
                name: "Consents");

            migrationBuilder.DropTable(
                name: "DoctorInformations");

            migrationBuilder.DropTable(
                name: "EmployeeInformations");

            migrationBuilder.DropTable(
                name: "HospitalInformations");

            migrationBuilder.DropTable(
                name: "BloodInventories");

            migrationBuilder.DropTable(
                name: "PatientRecords");

            migrationBuilder.DropTable(
                name: "DonorRecords");
        }
    }
}
