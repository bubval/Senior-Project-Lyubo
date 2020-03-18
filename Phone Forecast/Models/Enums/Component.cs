using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Forecast.Models.Enums
{
    public enum Component
    {
        [Display(Name = "CPU Speed")]
        CPUSpeed = 1,
        [Display(Name = "Internal Storage:")]
        InternalStorageSpace = 2,
        [Display(Name = "RAM")]
        RAM = 3,
        [Display(Name = "Front Camera Megapixel")]
        FrontCameraMegapixel = 4,
        [Display(Name = "Rear Camera Megapixel")]
        RearCameraMegapixel = 5,
        [Display(Name = "Max Framerate w/ Max Resolution")]
        MaxFramerateMaxResolution = 6,
        [Display(Name = "Max Framerate w/ Min Resolution")]
        MaxFramerateMinResolution = 7,
        [Display(Name = "Battery Capacity")]
        BatteryCapacity = 8,
        [Display(Name = "Volume")]
        Volume = 9,
        [Display(Name = "Price")]
        Price = 10
    }
}
