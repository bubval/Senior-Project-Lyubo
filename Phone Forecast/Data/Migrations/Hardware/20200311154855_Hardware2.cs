using Microsoft.EntityFrameworkCore.Migrations;

namespace Phone_Forecast.Data.Migrations.Hardware
{
    public partial class Hardware2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "HardwareConfigurations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HardwareConfigurations",
                table: "HardwareConfigurations",
                column: "ConfigId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HardwareConfigurations",
                table: "HardwareConfigurations");

            migrationBuilder.RenameTable(
                name: "HardwareConfigurations",
                newName: "Transactions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "ConfigId");
        }
    }
}
