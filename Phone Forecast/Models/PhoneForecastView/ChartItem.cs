using MoreLinq.Extensions;
using Phone_Forecast.Models.Forecasting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Phone_Forecast.Models.PhoneForecastView
{
    public class ChartItem
    {
        public ChartItem(string label, bool fill, int borderWidth, List<ForecastResult> data)
        {
            this.Label = label;
            this.Fill = fill;
            this.BorderWidth = borderWidth;
            this.Data = data;

            List<ForecastResult> orderedData;
            try
            {
                orderedData = data.OrderBy(p => p.Value).ToList();
            }
            catch (Exception e)
            {
                throw new Exception($"Could not order data: {e.Message}");
            }

            MinPrice = orderedData.First();
            MaxPrice = orderedData.Last();
        }

        public string Label { get; set; }
        public bool Fill { get; set; }
        public int BorderWidth { get; set; }
        public List<ForecastResult> Data { get; set; }
        public ForecastResult MinPrice;
        public ForecastResult MaxPrice;
    }
}
