using Phone_Forecast.Models.DbContexts;
using Phone_Forecast.Models.Forecasting;
using Phone_Forecast.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Phone_Forecast.Models.PhoneForecastView
{
    public class PhoneMetaInformation
    {
        public string PhoneName { get; private set; }
        public int PhoneId { get; private set; }
        public DateTime MaxDate { get; private set; }
        public double MaxPrice { get; private set; }
        public DateTime MinDate { get; private set; }
        public double MinPrice { get; private set; }

        public PhoneMetaInformation(Hardware hardware, List<ForecastResult> transactions, int forecastMonths)
        {
            PhoneName = hardware.PhoneModel.GetDisplayName();
            PhoneId = hardware.ConfigId;

            MinPrice = transactions.TakeLast(forecastMonths).Min(x => x.Value);
            MinDate = transactions.TakeLast(forecastMonths).Where(x => x.Value == MinPrice).Select(x => x.Date).FirstOrDefault();

            MaxPrice = transactions.TakeLast(forecastMonths).Max(x => x.Value);
            MaxDate = transactions.TakeLast(forecastMonths).Where(x => x.Value == MaxPrice).Select(x => x.Date).FirstOrDefault();
        }
    }
}
