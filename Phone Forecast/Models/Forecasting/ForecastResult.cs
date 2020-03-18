using System;

namespace Phone_Forecast.Models.Forecasting
{
    public class ForecastResult
    {
        public ForecastResult(DateTime date, double value)
        {
            Date = date;
            Value = value;
        }

        public DateTime Date { get; set; }
        public double Value { get; set; }
    }
}
