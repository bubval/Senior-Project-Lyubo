using System.ComponentModel.DataAnnotations;

namespace Phone_Forecast.Models.Enums
{
    public enum PhoneModel
    {
        [Display(Name = "Samsung Galaxy S9")]
        SamsungGalaxyS9 = 1,
        [Display(Name = "iPhone 8")]
        iPhone8 = 2,
    }
}