using Phone_Forecast.Models.DbContexts;
using System.Collections.Generic;

namespace Phone_Forecast.Models.PhoneForecastView
{
    public class PhoneForecast
    {
        public PhoneForecast(List<ChartItem> charts, List<Hardware> allHardware, List<int> selectedIds,
                             int futureForecastMonths, List<string> errors, List<PhoneMetaInformation> metaInfo)
        {
            this.Charts = charts;
            this.AllHardware = allHardware;
            this.SelectedIds = selectedIds;
            this.FutureForecastMonths = futureForecastMonths;
            this.Errors = errors;
            this.MetaInfo = metaInfo;
        }
        public List<ChartItem> Charts { get; private set; }
        public List<Hardware> AllHardware { get; private set; }
        public List<int> SelectedIds { get; private set; }
        public int FutureForecastMonths { get; private set; }
        public List<string> Errors { get; private set; }
        public List<PhoneMetaInformation> MetaInfo { get; private set; }
    }
}
