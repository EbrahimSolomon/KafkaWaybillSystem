using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaybillSearchApp.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Waybills_WaybillNumber",
                table: "Waybills",
                column: "WaybillNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_ParcelNumber",
                table: "Parcels",
                column: "ParcelNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Waybills_WaybillNumber",
                table: "Waybills");

            migrationBuilder.DropIndex(
                name: "IX_Parcels_ParcelNumber",
                table: "Parcels");
        }
    }
}
