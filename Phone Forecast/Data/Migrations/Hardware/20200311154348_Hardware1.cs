using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Phone_Forecast.Data.Migrations.Hardware
{
    public partial class Hardware1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    ConfigId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PhoneModel = table.Column<int>(nullable: false),
                    InternalStorageSpace = table.Column<int>(nullable: false),
                    HasMemoryCardReader = table.Column<bool>(nullable: false),
                    Cpu = table.Column<int>(nullable: false),
                    ProcessorCoreCount = table.Column<int>(nullable: false),
                    ProcessorCoreSpeed = table.Column<double>(nullable: false),
                    RAM = table.Column<int>(nullable: false),
                    HasGPU = table.Column<bool>(nullable: false),
                    GPU = table.Column<int>(nullable: false),
                    HasHeadphoneOutput = table.Column<bool>(nullable: false),
                    Has2g = table.Column<bool>(nullable: false),
                    Has3g = table.Column<bool>(nullable: false),
                    Has4g = table.Column<bool>(nullable: false),
                    Has5g = table.Column<bool>(nullable: false),
                    HasBluetooth = table.Column<bool>(nullable: false),
                    HasGps = table.Column<bool>(nullable: false),
                    HasWifi = table.Column<bool>(nullable: false),
                    HasRearCamera = table.Column<bool>(nullable: false),
                    HasFrontCamera = table.Column<bool>(nullable: false),
                    FrontCameraMegapixel = table.Column<int>(nullable: false),
                    RearCameraMegapixel = table.Column<int>(nullable: false),
                    MaximumLensAperture = table.Column<double>(nullable: false),
                    RearCameraCount = table.Column<int>(nullable: false),
                    CanRecordVideo = table.Column<bool>(nullable: false),
                    MaxFramerateMaxResolution = table.Column<int>(nullable: false),
                    MaxFramerateMinResolution = table.Column<int>(nullable: false),
                    BatteryCapacity = table.Column<int>(nullable: false),
                    HasExchangableBattery = table.Column<bool>(nullable: false),
                    Depth = table.Column<double>(nullable: false),
                    Height = table.Column<double>(nullable: false),
                    Width = table.Column<double>(nullable: false),
                    Weight = table.Column<double>(nullable: false),
                    HasWirelessCharging = table.Column<bool>(nullable: false),
                    WirelessStandard = table.Column<int>(nullable: false),
                    HasDualSim = table.Column<bool>(nullable: false),
                    SimCard = table.Column<int>(nullable: false),
                    HasFastCharging = table.Column<bool>(nullable: false),
                    IsWaterResistant = table.Column<bool>(nullable: false),
                    OriginalPrice = table.Column<double>(nullable: false),
                    ReleaseDate = table.Column<DateTime>(nullable: false),
                    ProductPage = table.Column<string>(maxLength: 256, nullable: true),
                    IsSelected = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.ConfigId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
