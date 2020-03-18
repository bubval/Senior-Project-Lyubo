using System;
using System.Collections;
using System.Collections.Generic;

namespace Phone_Forecast.Models.Forecasting
{
    public class Observation : IEnumerable<Observation>
    {
        public DateTime Date { get; private set; }
        public double? Value { get; private set; }
        public double? MovingAverage { get; set; }
        public double? CenteredMovingAverage { get; set; }
        public double? SeasonalIrregularity { get; set; }
        public double? Seasonality { get; set; }
        public double? Deseasonalized { get; set; }
        public double? Trend { get; set; }
        public double? Forecast { get; set; }

        public Observation(DateTime date, double? value = null, double? movingAverage = null, double? centeredMovingAverage = null,
            double? seaonalIrregularity = null, double? seasonality = null, double? deseasonalized = null, double? trend = null,
            double? forecast = null)
        {
            this.Date = date;
            this.Value = value;
            this.MovingAverage = movingAverage;
            this.CenteredMovingAverage = centeredMovingAverage;
            this.SeasonalIrregularity = seaonalIrregularity;
            this.Seasonality = seasonality;
            this.Deseasonalized = deseasonalized;
            this.Trend = trend;
            this.Forecast = forecast;
        }

        public IEnumerator<Observation> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
