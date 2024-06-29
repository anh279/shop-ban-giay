using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanGiay.Migrations
{
    public partial class initStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvokeDateTime",
                table: "Invoke",
                newName: "OrderDate");

            migrationBuilder.AddColumn<int>(
                name: "DaysToDeliver",
                table: "Invoke",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "Invoke",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Invoke",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "ShippingFee",
                table: "Invoke",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ShippingMethod",
                table: "Invoke",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Invoke",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.StatusId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoke_StatusId",
                table: "Invoke",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoke_Status_StatusId",
                table: "Invoke",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoke_Status_StatusId",
                table: "Invoke");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropIndex(
                name: "IX_Invoke_StatusId",
                table: "Invoke");

            migrationBuilder.DropColumn(
                name: "DaysToDeliver",
                table: "Invoke");

            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "Invoke");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Invoke");

            migrationBuilder.DropColumn(
                name: "ShippingFee",
                table: "Invoke");

            migrationBuilder.DropColumn(
                name: "ShippingMethod",
                table: "Invoke");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Invoke");

            migrationBuilder.RenameColumn(
                name: "OrderDate",
                table: "Invoke",
                newName: "InvokeDateTime");
        }
    }
}
