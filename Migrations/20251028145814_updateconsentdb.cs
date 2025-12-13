using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloodBankManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class updateconsentdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InventoryId",
                table: "Consents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Consents_InventoryId",
                table: "Consents",
                column: "InventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consents_BloodInventories_InventoryId",
                table: "Consents",
                column: "InventoryId",
                principalTable: "BloodInventories",
                principalColumn: "InventoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consents_BloodInventories_InventoryId",
                table: "Consents");

            migrationBuilder.DropIndex(
                name: "IX_Consents_InventoryId",
                table: "Consents");

            migrationBuilder.DropColumn(
                name: "InventoryId",
                table: "Consents");
        }
    }
}
