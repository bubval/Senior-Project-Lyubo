using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Phone_Forecast.Models.DbContexts;
using Phone_Forecast.Models.Enums;
using Phone_Forecast.Models.Forecasting;
using Phone_Forecast.Models.PhoneForecastView;
using Phone_Forecast.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Forecast.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly TransactionContext m_transactionContext;
        private readonly HardwareContext m_hardwareContext;
        public MemberController(TransactionContext transactionContext, HardwareContext hardwareContext)
        {
            m_transactionContext = transactionContext;
            m_hardwareContext = hardwareContext;
        }

        public async Task<IActionResult> Phone(List<int> selectedItems = null, int forecastMonths = 12)
        {
            List<Transaction> allTransactions = await m_transactionContext.Transactions.ToListAsync();
            List<Hardware> allHardware = m_hardwareContext.HardwareConfigurations.ToList();
            List<string> errors = new List<string>();
            List<PhoneMetaInformation> metaInfo = new List<PhoneMetaInformation>();
            List<ChartItem> allCharts = new List<ChartItem>();
            List<int> selectedIds = new List<int>();

            if (selectedItems != null)
                selectedIds = selectedItems;

            RemoveUnforecastable(ref selectedIds, ref errors);
            allCharts = GetCharts(selectedIds, allTransactions, forecastMonths);
            metaInfo = GetPhoneMetaInformation(selectedIds, allTransactions, forecastMonths);


            return View(new PhoneForecast(allCharts.Distinct().ToList(),
                                          allHardware.Distinct().ToList(),
                                          selectedIds,
                                          forecastMonths,
                                          errors.Distinct().ToList(),
                                          metaInfo.Distinct().ToList()));
        }

        public async Task<IActionResult> Info(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phone = await m_hardwareContext.HardwareConfigurations.FirstOrDefaultAsync(x => x.ConfigId == id);

            if (phone == null)
            {
                return NotFound();
            }

            return View(phone);
        }

        public async Task<IActionResult> Suggestion(List<int> ids = null, int forecastMonths = 12)
        {
            List<Transaction> allTransactions = await m_transactionContext.Transactions.ToListAsync();
            List<Hardware> allHardware = await m_hardwareContext.HardwareConfigurations.ToListAsync();
            List<ChartItem> charts = new List<ChartItem>();
            List<string> errors = new List<string>();
            List<int> selectedIds = new List<int>();
            Dictionary<string, double> scores = new Dictionary<string, double>();

            if (ids != null)
                selectedIds = ids;

            RemoveUnforecastable(ref selectedIds, ref errors);

            List<ChartItem> forecastableCharts = GetCharts(selectedIds, allTransactions, forecastMonths);
            if (forecastableCharts.Count > 0)
                charts = forecastableCharts;

            // No need to show scores with a single product
            if (forecastableCharts.Count > 1)
                scores = GetScores(selectedIds);

            // - Add Transactions (possibly?)
            return View(new PhoneSuggestion(charts, allHardware, selectedIds, forecastMonths, scores));
        }

        public async Task<IActionResult> Moore(Component component = Component.CPUSpeed, int months = 12)
        {
            // Component, Hardware Chart, Moore Chart
            List<Hardware> allHardware = await m_hardwareContext.HardwareConfigurations.ToListAsync();
            Tuple<Component, ChartItem, ChartItem> Charts = GetMooreCharts(component, allHardware, months);

            return View(new Moore(Charts, months));
        }

        #region Redirects
        public async Task<IActionResult> InputSpecs(int? storage, bool? hasMemoryCardReader, int? cpuCoreCount, double? cpuSpeed,
           int? ram, bool? headphoneOutput, bool? is5gCapable, int? frontCameraMgpx, int? backCameraMgpx,
           int? rearCameraCount, bool? exchangableBattery, bool? wirelessCharging, bool? fastCharging, bool? waterResistance)
        {
            List<Hardware> phones = new List<Hardware>();
            phones = await m_hardwareContext.HardwareConfigurations.ToListAsync();

            if (hasMemoryCardReader.HasValue)
                phones = phones.Where(x => x.HasMemoryCardReader == hasMemoryCardReader).ToList();
            if (headphoneOutput.HasValue)
                phones = phones.Where(x => x.HasHeadphoneOutput == headphoneOutput).ToList();
            if (is5gCapable.HasValue)
                phones = phones.Where(x => x.Has5g == is5gCapable).ToList();
            if (exchangableBattery.HasValue)
                phones = phones.Where(x => x.HasExchangableBattery == exchangableBattery).ToList();
            if (wirelessCharging.HasValue)
                phones = phones.Where(x => x.HasWirelessCharging == wirelessCharging).ToList();
            if (fastCharging.HasValue)
                phones = phones.Where(x => x.HasFastCharging == fastCharging).ToList();
            if (waterResistance.HasValue)
                phones = phones.Where(x => x.IsWaterResistant == waterResistance).ToList();

            if (storage.HasValue)
                phones = phones.Where(x => x.InternalStorageSpace >= storage).ToList();
            if (cpuCoreCount.HasValue)
                phones = phones.Where(x => x.ProcessorCoreCount >= cpuCoreCount).ToList();
            if (cpuSpeed.HasValue)
                phones = phones.Where(x => x.ProcessorCoreSpeed >= cpuSpeed).ToList();
            if (ram.HasValue)
                phones = phones.Where(x => x.RAM >= ram).ToList();
            if (frontCameraMgpx.HasValue)
                phones = phones.Where(x => x.FrontCameraMegapixel >= frontCameraMgpx).ToList();
            if (backCameraMgpx.HasValue)
                phones = phones.Where(x => x.RearCameraMegapixel >= backCameraMgpx).ToList();
            if (rearCameraCount.HasValue)
                phones = phones.Where(x => x.RearCameraCount >= rearCameraCount).ToList();

            List<int> ids = phones.Select(x => x.ConfigId).ToList();
            return RedirectToAction("Suggestion", "Member", new { ids });
        }

        [HttpPost]
        public IActionResult ChangeForecastMonths(string selectedItems, string forecastMonths)
        {
            List<int> allIds = JsonConvert.DeserializeObject<List<int>>(selectedItems);
            int forecast = JsonConvert.DeserializeObject<int>(forecastMonths);
            return RedirectToAction("Phone", "Member", new { selectedItems = allIds, forecastMonths = forecast });
        }

        public IActionResult AddToChart(string forecastMonths, string currentItem, string selectedItems = null)
        {
            List<int> selectedHardwareIds = new List<int>();
            int currentlySelectedId = JsonConvert.DeserializeObject<int>(currentItem);
            int forecast = JsonConvert.DeserializeObject<int>(forecastMonths);

            if (selectedItems != null)
            {
                selectedHardwareIds = JsonConvert.DeserializeObject<List<int>>(selectedItems);
            }
            selectedHardwareIds.Add(currentlySelectedId);

            return RedirectToAction("Phone", "Member", new { selectedItems = selectedHardwareIds, forecastMonths = forecast });
        }

        public IActionResult RemoveFromChart(string currentItem, string selectedItems, string forecastMonths)
        {
            List<int> allHardwareIds = JsonConvert.DeserializeObject<List<int>>(selectedItems);
            int currentlySelectedId = JsonConvert.DeserializeObject<int>(currentItem);
            int forecast = JsonConvert.DeserializeObject<int>(forecastMonths);

            if (allHardwareIds.Contains(currentlySelectedId))
            {
                allHardwareIds.RemoveAll(x => x == currentlySelectedId);
            }

            return RedirectToAction("Phone", "Member", new { selectedItems = allHardwareIds, forecastMonths = forecast });
        }

        public IActionResult ChangeMooreForecast(string component, string months)
        {
            Component comp = JsonConvert.DeserializeObject<Component>(component);
            int futureForecastMonths = JsonConvert.DeserializeObject<int>(months);

            return RedirectToAction("Moore", "Member", new { component = comp, months = futureForecastMonths});
        }
        #endregion

        #region Private Functions
        private void RemoveUnforecastable(ref List<int> ids, ref List<string> errors)
        {
            List<int> sentIds = ids;
            foreach (int id in sentIds.ToList())
            {
                if (!m_hardwareContext.HardwareConfigurations.Any(x => x.ConfigId == id))
                {
                    ids.Remove(id);
                    errors.Add($"No hardware could be found for id {id}.");
                    continue;
                }

                Hardware hardware = m_hardwareContext.HardwareConfigurations.Where(x => x.ConfigId == id).First();

                if (m_transactionContext.Transactions.Where(x => x.Phone == hardware.PhoneModel).Count() < 24)
                {
                    ids.Remove(id);
                    errors.Add($"{hardware.PhoneModel.GetDisplayName()} - Not enough transactions on record.");
                    continue;
                }

                List<Transaction> allTransactions = m_transactionContext.Transactions.Where(x => x.Phone == hardware.PhoneModel).ToList();
                DateTime earliestDate = allTransactions.Min(x => x.PurchaseDate);
                DateTime latestDate = allTransactions.Max(x => x.PurchaseDate);
                DateTimeSpan dateTimeSpan = DateTimeSpan.CompareDates(earliestDate, latestDate);
                int? monthDifference = (dateTimeSpan.Years * 12) + dateTimeSpan.Days;

                if (monthDifference == null || monthDifference < 24)
                {
                    ids.Remove(id);
                    errors.Add($"{hardware.PhoneModel.GetDisplayName()} - Month difference between the first and last date is not large enough.");
                    continue;
                }
            }
        }

        private List<ChartItem> GetCharts(List<int> ids, List<Transaction> transactions, int months)
        {
            List<ChartItem> charts = new List<ChartItem>();
            foreach (int id in ids.Distinct())
            {
                if (!m_hardwareContext.HardwareConfigurations.Any(x => x.ConfigId == id))
                    continue;

                Hardware phone = m_hardwareContext.HardwareConfigurations.Where(x => x.ConfigId == id).First();

                if (phone == null)
                    continue;

                TimeSeriesForecast tsf = new TimeSeriesForecast(transactions, phone.PhoneModel);

                if (tsf.IsForecastable())
                {
                    DateTime today = DateTime.Today;
                    DateTime twoYearsAgo = new DateTime(today.Year - 2, today.Month, 1);
                    var forecast = tsf.GenerateFutureForecast(months);
                    charts.Add(new ChartItem(phone.PhoneModel.GetDisplayName(),
                                                false,
                                                2,
                                                forecast.Where(x => x.Date >= twoYearsAgo).ToList()));
                }
            }
            return charts;
        }

        private Tuple<Component, ChartItem, ChartItem> GetMooreCharts(Component component, List<Hardware> allHardware, int forecastMonths)
        {
            HardwareForecast hardwareForecast = new HardwareForecast(component, allHardware);
            List<ForecastResult> hardwareResult = hardwareForecast.GenerateFutureForecast(forecastMonths);

            MooreForecasting mooreForecast = new MooreForecasting(component, allHardware, forecastMonths);
            List<ForecastResult> mooreResult = mooreForecast.GetResults();

            ChartItem hardwareChart = new ChartItem(component.GetDisplayName(),
                                                    false,
                                                    2,
                                                    hardwareResult.ToList());
            ChartItem mooreChart = new ChartItem(component.GetDisplayName() + "Moore",
                                                 false,
                                                 2,
                                                 mooreResult.Where(x => x.Date >= hardwareResult.Min(y => y.Date)).ToList());
            return (new Tuple<Component, ChartItem, ChartItem>(component, hardwareChart, mooreChart));
        }

        private List<PhoneMetaInformation> GetPhoneMetaInformation(List<int> ids, List<Transaction> transactions, int months)
        {
            List<PhoneMetaInformation> metas = new List<PhoneMetaInformation>();

            foreach (int id in ids.Distinct())
            {
                if (!m_hardwareContext.HardwareConfigurations.Any(x => x.ConfigId == id))
                    continue;

                Hardware phone = m_hardwareContext.HardwareConfigurations.Where(x => x.ConfigId == id).First();

                if (phone == null)
                    continue;

                TimeSeriesForecast tsf = new TimeSeriesForecast(transactions, phone.PhoneModel);

                if (tsf.IsForecastable())
                {
                    var forecast = tsf.GenerateFutureForecast(months);
                    metas.Add(new PhoneMetaInformation(phone, forecast, months));
                }
            }

            return metas;
        }

        private Dictionary<string, double> GetScores(List<int> ids)
        {
            Dictionary<string, double> dictionary = new Dictionary<string, double>();

            // At this point, preprocessing has happened and all phones have been forecasted.
            foreach (int id in ids)
            {
                Hardware current = m_hardwareContext.HardwareConfigurations.FirstOrDefault(x => x.ConfigId == id);
                PhoneModel model = current.PhoneModel;
                List<Hardware> allOther = m_hardwareContext.HardwareConfigurations.Where(x => x.ConfigId != id).ToList();
                double score = CalculateScore(current, allOther);
                dictionary.Add(model.GetDisplayName(), score);
            }

            return dictionary;
        }

        private double CalculateScore(Hardware current, List<Hardware> allOther)
        {
            List<double> score = new List<double>();
            var transactionsDb = m_transactionContext.Transactions.ToList();
            var currentTransactions = transactionsDb.Where(x => x.Phone == current.PhoneModel).ToList();

            int currentPrice = 0;
            if (currentTransactions.Count > 23)
                currentPrice = (int)currentTransactions.Select(x => x.Price).Average();


            foreach (Hardware other in allOther)
            {
                List<double> oneToOneScore = new List<double>();

                var otherTransactions = transactionsDb.Where(x => x.Phone == other.PhoneModel).ToList();
                var otherPrice = 0;
                if (otherTransactions.Count > 23)
                    otherPrice = (int)otherTransactions.Select(x => x.Price).Average();

                if(currentPrice !=0 && otherPrice != 0)
                {
                    var difference = PercentDiff(currentPrice, otherPrice);
                    if (difference > 0)
                    {
                        oneToOneScore.Add(50);
                    }
                    else
                    {
                        oneToOneScore.Add(-50);
                    }
                }

                int mainStorage = current.InternalStorageSpace;
                int compStorage = other.InternalStorageSpace;
                oneToOneScore.Add(PercentDiff(mainStorage, compStorage));

                int mainCpuCount = current.ProcessorCoreCount;
                int compCpuCount = other.ProcessorCoreCount;
                oneToOneScore.Add(PercentDiff(mainCpuCount, compCpuCount));

                double mainCpuSpeed = current.ProcessorCoreSpeed;
                double compCpuSpeed = other.ProcessorCoreSpeed;
                oneToOneScore.Add(PercentDiff(mainCpuSpeed, compCpuSpeed));

                int mainRam = current.RAM;
                int compRam = other.RAM;
                oneToOneScore.Add(PercentDiff(mainRam, compRam));

                int mainFrontCameraMgpx = current.FrontCameraMegapixel;
                int compFrontCameraMgpx = other.FrontCameraMegapixel;
                oneToOneScore.Add(PercentDiff(mainFrontCameraMgpx, compFrontCameraMgpx));

                int mainBackCameraMgpx = current.RearCameraMegapixel;
                int compBackCameraMgpx = other.RearCameraMegapixel;
                oneToOneScore.Add(PercentDiff(mainBackCameraMgpx, compBackCameraMgpx));

                int mainRearCamera = current.RearCameraCount;
                int compRearCameraMgpx = other.RearCameraCount;
                oneToOneScore.Add(PercentDiff(mainRearCamera, compRearCameraMgpx));

                // Adds a hardware-to-hardware score for the current phone.
                score.Add(oneToOneScore.Average());
            }

            var finalScore = score.Average();

            if (finalScore < 0)
                return 0;
            else
                return (int)score.Average();
        }

        private double PercentDiff(double number, double comparisonNumber)
        {
            return (((number - comparisonNumber) / Math.Abs(comparisonNumber)) * 100);
        }
        #endregion
    }
}