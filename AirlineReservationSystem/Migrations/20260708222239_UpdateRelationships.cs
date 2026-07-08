using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirlineReservationSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baggages_Bookings_BookingId",
                table: "Baggages");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Bookings_BookingId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Passengers_PassengerId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_BookingId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_PassengerId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "PassengerId",
                table: "Tickets",
                newName: "BookingPassengerId");

            migrationBuilder.RenameColumn(
                name: "BookingId",
                table: "Baggages",
                newName: "PassengerId");

            migrationBuilder.RenameIndex(
                name: "IX_Baggages_BookingId",
                table: "Baggages",
                newName: "IX_Baggages_PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_BookingPassengerId",
                table: "Tickets",
                column: "BookingPassengerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Baggages_Passengers_PassengerId",
                table: "Baggages",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_BookingPassengers_BookingPassengerId",
                table: "Tickets",
                column: "BookingPassengerId",
                principalTable: "BookingPassengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baggages_Passengers_PassengerId",
                table: "Baggages");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_BookingPassengers_BookingPassengerId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_BookingPassengerId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "BookingPassengerId",
                table: "Tickets",
                newName: "PassengerId");

            migrationBuilder.RenameColumn(
                name: "PassengerId",
                table: "Baggages",
                newName: "BookingId");

            migrationBuilder.RenameIndex(
                name: "IX_Baggages_PassengerId",
                table: "Baggages",
                newName: "IX_Baggages_BookingId");

            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_BookingId",
                table: "Tickets",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PassengerId",
                table: "Tickets",
                column: "PassengerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Baggages_Bookings_BookingId",
                table: "Baggages",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Bookings_BookingId",
                table: "Tickets",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Passengers_PassengerId",
                table: "Tickets",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
