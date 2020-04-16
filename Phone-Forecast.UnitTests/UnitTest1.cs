using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phone_Forecast.Utilities;
using Phone_Forecast.Models.Enums;
using System;
using Phone_Forecast.Models.DbContexts;
using System.Linq;
using System.Threading.Tasks;
using Phone_Forecast.Models.Forecasting;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Phone_Forecast.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private DbContextOptionsBuilder<TransactionContext> _tOptionsBuilder;
        private DbContextOptionsBuilder<HardwareContext> _hOptionsBuilder;

        private DbContextOptions<TransactionContext> _tOptions;
        private DbContextOptions<HardwareContext> _hOptions;

        private TransactionContext _tContext;
        private HardwareContext _hContext;

        [TestInitialize]
        public void Setup()
        {
            _tOptionsBuilder = new DbContextOptionsBuilder<TransactionContext>().UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = TransactionsDatabase; Trusted_Connection = True; MultipleActiveResultSets = true");
            _tOptions = _tOptionsBuilder.Options;
            _tContext = new TransactionContext(_tOptions);

            _hOptionsBuilder = new DbContextOptionsBuilder<HardwareContext>().UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=HardwareDatabase;Trusted_Connection=True;MultipleActiveResultSets=true");
            _hOptions = _hOptionsBuilder.Options;
            _hContext = new HardwareContext(_hOptions);
        }

        /*
         * UNIT TESTS
         * 
         */

        [TestMethod]
        public void DateTimeSpanMonths()
        {
            // Arrange
            DateTime dt1 = new DateTime(2000, 1, 1);
            DateTime dt2 = new DateTime(2000, 2, 28);

            // Act
            var dts = DateTimeSpan.CompareDates(dt1, dt2).Months;

            // Asset
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(dts, 1);
        }

        [TestMethod]
        public void DateTimeSpanDays()
        {
            // Arrange
            DateTime dt1 = new DateTime(2000, 1, 1);
            DateTime dt2 = new DateTime(2000, 1, 3);

            // Act
            var dts = DateTimeSpan.CompareDates(dt1, dt2).Days;

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(dts, 2);
        }

        [TestMethod]
        public void DateTimeYears()
        {
            //Arrange
            DateTime dt1 = new DateTime(2000, 1, 1);
            DateTime dt2 = new DateTime(2001, 1, 1);

            // Act
            var dts = DateTimeSpan.CompareDates(dt1, dt2).Years;

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(dts, 1);
        }

        [TestMethod]
        public void GetDescriptionExtension()
        {
            // Arrange
            var component = Component.BatteryCapacity;

            // Act
            string bc = component.GetDisplayName();

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(bc, "Battery Capacity");
        }

        /*
         * INTEGRATION TESTS
         * 
         */

        [TestMethod]
        public void GetTransactions()
        {
            // Arrange
            var transactions = _tContext.Transactions.ToList();

            // Act
            var count = transactions.Count();

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void GetHardware()
        {
            // Arrange
            var hardware = _hContext.HardwareConfigurations.ToList();

            // Act
            var count = hardware.Count();

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void AddTransactions()
        {
            // Arrange
            var transactions = _tContext.Transactions.ToList();
            var count1 = transactions.Count();
            
            // Act
            transactions.Add(new Transaction() {Phone = PhoneModel.iPhone8, Price = 0.0, PurchaseDate = new DateTime(2002, 1, 1) });
            var count2 = transactions.Count();
            
            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(count2 == (count1 + 1));
        }

        [TestMethod]
        public void AddHardware()
        {
            // Arrange
            var hardware = _hContext.HardwareConfigurations.ToList();
            var count1 = hardware.Count();

            // Act
            hardware.Add(new Hardware()
            {
                BatteryCapacity = 3000,
                CanRecordVideo = true,
                Cpu = CPU.AppleA11Bionic,
                Depth = 10.1,
                FrontCameraMegapixel = 10,
                GPU = GPU.ARM_Mail_G72_MP12,
                Has2g = true,
                Has3g = true,
                Has4g = true,
                Has5g = true,
                HasBluetooth = true,
                HasDualSim = true,
                HasExchangableBattery = true,
                HasFastCharging = true,
                HasFrontCamera = true,
                HasGps = true,
                HasGPU = true,
                HasHeadphoneOutput = true,
                HasMemoryCardReader = true,
                HasRearCamera = true,
                HasWifi = true,
                HasWirelessCharging = true,
                Height = 20.0,
                InternalStorageSpace = 24,
                IsSelected = true,
                IsWaterResistant = true,
                OriginalPrice = 99,
                MaxFramerateMaxResolution = 60,
                MaxFramerateMinResolution = 60,
                MaximumLensAperture = 12,
                PhoneModel = PhoneModel.iPhone8,
                RAM = 4,
                ProcessorCoreCount = 4,
                ProcessorCoreSpeed = 1.3,
                ProductPage = "",
                RearCameraCount = 4,
                RearCameraMegapixel = 13,
                ReleaseDate = new DateTime(2000, 1, 1),
                SimCard = SimCard.Nano,
                Weight = 45.0,
                Width = 32.0,
                WirelessStandard = WirelessStandard.QI
            });
            var count2 = hardware.Count();

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(count2 == (count1 + 1));
        }

        [TestMethod]
        public void DeleteTransaction()
        {
            // Arrange
            var transactions = _tContext.Transactions.ToList();
            var count1 = transactions.Count();
            // Act

            if (count1 > 0)
            {
                transactions.RemoveAt(0);
            }

            var count2 = transactions.Count();

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(count1 > count2);

        }

        [TestMethod]
        public void DeleteHardware()
        {
            // Arrange
            var hardware = _hContext.HardwareConfigurations.ToList();
            var count1 = hardware.Count();

            //Act
            if (count1 > 0)
            {
                hardware.RemoveAt(0);
            }

            var count2 = hardware.Count();

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(count1 > count2);
        }

        [TestMethod]
        public void UpdateHardware()
        {
            // Arrange
            var hardware = _hContext.HardwareConfigurations;
            var count = hardware.ToList().Count();

            if (count < 0)
            {
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail();
            }

            // Act
            var foo = hardware.ToList().ElementAt(0);
            foo.RAM = 0;
            hardware.Update(foo);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(hardware.ToList().ElementAt(0).RAM == 0);
        }

        [TestMethod]
        public void UpdateTransaction()
        {
            // Arrange
            var transactions = _tContext.Transactions.ToList();
            var count = transactions.Count();

            if (count < 0)
            {
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail();
            }

            // Act
            var foo = transactions.ElementAt(0);
            foo.Price = 0;

            _tContext.Transactions.Update(foo);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(transactions.ElementAt(0).Price == 0);

        }


        [TestMethod]
        public void TimeSeriesForecast()
        {
            // Arrange
            var transactions = _tContext.Transactions.ToList();

            // Act
            var tsf = new TimeSeriesForecast(transactions, PhoneModel.iPhone8);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(tsf.IsForecastable() == true);
        }

        [TestMethod]
        public void HardwareForecast()
        {
            // Arrange
            var hardware = _hContext.HardwareConfigurations.ToList();

            // Act
            var hwf = new HardwareForecast(Component.CPUSpeed, hardware);
            var foo = hwf.GenerateFutureForecast();

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(foo.Count > 23);
        }

        [TestMethod]
        public void MooreForecast()
        {
            // Arrange
            var hardware = _hContext.HardwareConfigurations.ToList();

            // Act
            var mf = new MooreForecasting(Component.CPUSpeed, hardware);
            var foo = mf.GetResults();

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(foo.Count > 23);
        }
    }
}
