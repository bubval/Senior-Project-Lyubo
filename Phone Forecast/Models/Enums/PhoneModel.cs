using System.ComponentModel.DataAnnotations;

namespace Phone_Forecast.Models.Enums
{
    public enum PhoneModel
    {
        [Display(Name = "Samsung Galaxy S9")]
        SamsungGalaxyS9 = 1,
        [Display(Name = "Test")]
        iPhone8 = 2,
        [Display(Name = "Samsung Galaxy S7")]
        SamsungGalaxyS7 = 3,
        [Display(Name = "iPhone X")]
        iPhone10 = 4,
        [Display(Name = "Samsung Galaxy S8")]
        SamsungGalaxyS8 = 5,
        [Display(Name = "iPhone 6S")]
        iPhone6S = 6
    }
}