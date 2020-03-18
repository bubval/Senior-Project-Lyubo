using Phone_Forecast.Models.DbContexts;
using Phone_Forecast.Models.Enums;
using System.Collections.Generic;

namespace Phone_Forecast.Models.PhoneForecastView
{
    public class PhoneSuggestion
    {
        public PhoneSuggestion(List<ChartItem> charts, List<Hardware> allHardware, List<int> selectedIds, int futureForecastMonths, Dictionary<PhoneModel, double> scores)
        {
            this.Charts = charts;
            this.AllHardware = allHardware;
            this.SelectedIds = selectedIds;
            this.FutureForecastMonths = futureForecastMonths;
            this.Scores = scores;
        }

        public List<ChartItem> Charts { get; private set; }
        public List<Hardware> AllHardware { get; private set; }
        public List<int> SelectedIds { get; private set; }
        public int FutureForecastMonths { get; private set; }
        public Dictionary<PhoneModel, double> Scores { get; private set; }
    }
}
