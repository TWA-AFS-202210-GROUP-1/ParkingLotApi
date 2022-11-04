using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingLotApi.Migrations
{
    public partial class _3rd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingOrders_ParkingLots_ParkingLotId",
                table: "ParkingOrders");

            migrationBuilder.DropIndex(
                name: "IX_ParkingOrders_ParkingLotId",
                table: "ParkingOrders");

            migrationBuilder.DropColumn(
                name: "ParkingLotId",
                table: "ParkingOrders");

            migrationBuilder.AddColumn<int>(
                name: "ParkingLotEntityId",
                table: "ParkingOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParkingOrders_ParkingLotEntityId",
                table: "ParkingOrders",
                column: "ParkingLotEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingOrders_ParkingLots_ParkingLotEntityId",
                table: "ParkingOrders",
                column: "ParkingLotEntityId",
                principalTable: "ParkingLots",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingOrders_ParkingLots_ParkingLotEntityId",
                table: "ParkingOrders");

            migrationBuilder.DropIndex(
                name: "IX_ParkingOrders_ParkingLotEntityId",
                table: "ParkingOrders");

            migrationBuilder.DropColumn(
                name: "ParkingLotEntityId",
                table: "ParkingOrders");

            migrationBuilder.AddColumn<int>(
                name: "ParkingLotId",
                table: "ParkingOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ParkingOrders_ParkingLotId",
                table: "ParkingOrders",
                column: "ParkingLotId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingOrders_ParkingLots_ParkingLotId",
                table: "ParkingOrders",
                column: "ParkingLotId",
                principalTable: "ParkingLots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
