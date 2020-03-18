using Phone_Forecast.Models.Enums;
using Phone_Forecast.Models.Forecasting;
using System;
using System.Collections.Generic;

namespace Phone_Forecast.Models.PhoneForecastView
{
    public class Moore
    {
        public Tuple<Component, ChartItem, ChartItem> Charts;
        public int FutureForecastMonths;

        public Moore(Tuple<Component, ChartItem, ChartItem> charts, int futureForecastMonths = 12)
        {
            Charts = charts;
            FutureForecastMonths = futureForecastMonths;
        }
    }
}
