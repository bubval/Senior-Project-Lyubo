using Microsoft.EntityFrameworkCore.Migrations;

namespace Phone_Forecast.Data.Migrations.Transaction
{
    public partial class Transaction2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Transactions");

            migrationBuilder.AddColumn<int>(
                name: "Phone",
                table: "Transactions",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Transactions");

            migrationBuilder.AddColumn<int>(
                name: "Brand",
                table: "Transactions",
                nullable: false,
                defaultValue: 0);
        }
    }
}
