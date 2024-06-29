using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanGiay.Migrations
{
    public partial class initKeyInvokeDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvokeDetailId",
                table: "InvokeDetail",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvokeDetail",
                table: "InvokeDetail",
                column: "InvokeDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_InvokeDetail",
                table: "InvokeDetail");

            migrationBuilder.DropColumn(
                name: "InvokeDetailId",
                table: "InvokeDetail");
        }
    }
}
