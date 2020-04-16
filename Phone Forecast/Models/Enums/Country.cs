using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Forecast.Models.Enums
{
    public enum Country
    {
        [Description("Bulgaria")]
        BG = 1,
        [Description("United States")]
        USA,
        [Description("United Kingdom")]
        UK
    }
}
