using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirlineReservationSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditeBaggageModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baggages_Flights_FlightId",
                table: "Baggages");

            migrationBuilder.DropIndex(
                name: "IX_Baggages_FlightId",
                table: "Baggages");

            migrationBuilder.DropColumn(
                name: "BagTagNumber",
                table: "Baggages");

            migrationBuilder.DropColumn(
                name: "FlightId",
                table: "Baggages");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Baggages");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "Baggages",
                newName: "ExtraWeightKg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExtraWeightKg",
                table: "Baggages",
                newName: "Weight");

            migrationBuilder.AddColumn<string>(
                name: "BagTagNumber",
                table: "Baggages",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FlightId",
                table: "Baggages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Baggages",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Baggages_FlightId",
                table: "Baggages",
                column: "FlightId");

            migrationBuilder.AddForeignKey(
                name: "FK_Baggages_Flights_FlightId",
                table: "Baggages",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
