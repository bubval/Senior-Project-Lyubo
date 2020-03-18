using Phone_Forecast.Models.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Phone_Forecast.Models.DbContexts
{
    public class Hardware
    {
        [Key]
        public int ConfigId { get; set; }

        /*
         *  /Basic Hardware Information/
         *  
         *  PhoneModel - Samsung Galaxy Edge 4, iPhone 8, Huawei P20
         */
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Phone Model:")]
        public PhoneModel PhoneModel { get; set; }

        /*
         *  /Storage Information/
         *  
         *  InternalStorageSpace (size in gigabytes) - 64, 128, 256 
         *  HasMemoryCardReader - Yes / No
         */
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Internal Storage:")]
        [Range(1, 1024, ErrorMessage = "Storage should be between 1 and 1024 Gigabytes.")]
        public int InternalStorageSpace { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Memory Card:")]
        [DefaultValue(false)]
        public bool HasMemoryCardReader { get; set; }

        /*
         *  /CPU (System on Chip) Information/
         *  
         *  CPU - HiSilicon Kirin 970
         *  ProcessorCoreCount - 2, 4, 6, 8
         *  ProcessorSpeed (in GHz) - 2.36
         */
        [DisplayName("CPU:")]
        public CPU Cpu { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("CPU Core Count:")]
        [Range(1, 16, ErrorMessage = "CPU Cores should be between 1 and 16")]
        public int ProcessorCoreCount { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Single Core Speed:")]
        [Range(0.1, 5.0, ErrorMessage = "Single core speed should be between 0.1 and 5.0 GHz")]
        public double ProcessorCoreSpeed { get; set; }

        /*
         *  /Memory Infomration/
         *  
         *  RAM (RAM size in Gigabytes) - 2, 4
         */
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("RAM:")]
        [Range(1, 64, ErrorMessage = "RAM should be between 1 and 64 gigabytes.")]
        public int RAM { get; set; }

        /*
         *  /Graphical Processing Information/
         *  
         *  HasGPU - Yes / No
         *  GPU - ARM Mali-G72 MP12
         */
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Has GPU:")]
        public bool HasGPU { get; set; }

        [DisplayName("GPU:")]
        public GPU GPU { get; set; }

        /*
         *  /Connectors and Communication Information/
         *  
         *  HasHeadphoneOutput - Yes / No
         *  Has2g - Yes / No
         *  Has3g - Yes / No
         *  Has4g - Yes / No
         *  Has5g - Yes / No
         *  HasBluetooth - Yes / No
         *  HasGps - Yes / No
         *  HasWifi - Yes / No
         */
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Headphone Output:")]
        public bool HasHeadphoneOutput { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("2G:")]
        [DefaultValue(true)]
        public bool Has2g { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("3G:")]
        [DefaultValue(true)]
        public bool Has3g { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("4G:")]
        public bool Has4g { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("5G:")]
        public bool Has5g { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Bluetooth:")]
        [DefaultValue(true)]
        public bool HasBluetooth { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("GPS:")]
        [DefaultValue(true)]
        public bool HasGps { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("WiFi:")]
        [DefaultValue(true)]
        public bool HasWifi { get; set; }

        /*
         *  /Camera Information/
         *  
         *  HasRearCamera - Yes / No
         *  HasFrontCamera - Yes / No
         *  FrontCameraMegapixels  - 12
         *  RearCameraMegapixels - 20
         *  MaximumLensAperture - 1.8
         *  RearCameraCount - 4
         *  CanRecordVideo - Yes / No
         *  MaxFramerateMaxResolution (in FPS) - 30
         *  MaxFramerateMinResolution (in FPS) - 120
         */
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Has Rear Camera:")]
        [DefaultValue(true)]
        public bool HasRearCamera { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Has Front Camera:")]
        [DefaultValue(true)]
        public bool HasFrontCamera { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Front Camera MP:")]
        [Range(0, 40, ErrorMessage = "Front camera megapixels should be between 0 and 40.")]
        public int FrontCameraMegapixel { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Rear Camera MP:")]
        [Range(0, 40, ErrorMessage = "Rear camera megapixels should be between 0 and 40.")]
        public int RearCameraMegapixel { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Lens Aperture:")]
        [Range(0, 5.0, ErrorMessage = "Maximum lens aperture should be between 0 and 5.")]
        public double MaximumLensAperture { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Rear Camera Count:")]
        [Range(0, 8, ErrorMessage = "Rear camera count should be between 0 and 8")]
        public int RearCameraCount { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Records Video:")]
        [DefaultValue(true)]
        public bool CanRecordVideo { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Max Resolution Framerate:")]
        [Range(0, 120, ErrorMessage = "Framerate at max resolution should be between 0 and 120 FPS.")]
        public int MaxFramerateMaxResolution { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Min Resolution Framerate:")]
        [Range(0, 240, ErrorMessage = "Framerate at min resolution should be between 0 and 240 FPS.")]
        public int MaxFramerateMinResolution { get; set; }

        /*  /Battery Information/
         *  
         *  BatteryCapacity (in mAh) - 3400 
         *  HasExchangableBattery - Yes / No
         */
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Battery Capacity:")]
        [Range(1, 10000, ErrorMessage = "Battery capacity should be between 1 and 3400 mAh.")]
        public int BatteryCapacity { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Exchangable Battery:")]
        public bool HasExchangableBattery { get; set; }

        /*
         *  /Dimension Information/
         *  
         *  Depth (in mm) - 7.65
         *  Height (in mm) - 149.1
         *  Width (in mm) - 70.8
         *  Weight (in gram) - 165.0
         */
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Depth:")]
        [Range(5.0, 20.0, ErrorMessage = "Depth should be between 5 and 20 mm.")]
        public double Depth { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Height:")]
        [Range(50, 300, ErrorMessage = "Height should be between 50 and 300 mm.")]
        public double Height { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Width:")]
        [Range(30, 120, ErrorMessage = "Width should be between 30 and 120 mm.")]
        public double Width { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Weight:")]
        [Range(70, 300, ErrorMessage = "Weight should be between 70 and 300 grams.")]
        public double Weight { get; set; }

        /*
         *  /Misc Information/
         *  
         *  HasWirelessCharging - Yes / No
         *  WirelessStandard - QI
         *  HasDualSim - Yes / No
         *  SimCard - Nano
         *  HasFastCharging - Yes / No
         *  IsWaterResistant - Yes / No
         */
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Wireless Charging:")]
        public bool HasWirelessCharging { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Wireless Standard:")]
        public WirelessStandard WirelessStandard { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Dual Sim:")]
        public bool HasDualSim { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Sim Card:")]
        public SimCard SimCard { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Fast Charging:")]
        public bool HasFastCharging { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Water Resistance:")]
        public bool IsWaterResistant { get; set; }

        /*
         *  /Release/
         *  
         *  OriginalPrice (in USD) - 399.99
         *  ReleaseData - 01/01/2000
         *  ProductPage - https://www.apple.com/bg/iphone-8/specs/
         */
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Original Price:")]
        [Range(1, 3000, ErrorMessage = "Price should be between 1 and 3000 USD.")]
        public double OriginalPrice { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Original Release Date:")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "01/01/2008", "01/01/2020",
        ErrorMessage = "Valid dates for the Property {0} between {1} and {2}")]
        public DateTime ReleaseDate { get; set; }

        [DisplayName("Product Link:")]
        [StringLength(256)]
        [DataType(DataType.Url)]
        public string ProductPage { get; set; }

        /*
         *  /User Interaction/
         *  
         *  IsSelected - Yes / No
         */
        public bool IsSelected { get; set; }
    }
}
