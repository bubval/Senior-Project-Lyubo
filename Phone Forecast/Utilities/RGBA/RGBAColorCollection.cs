using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Forecast.Utilities.RGBA
{
    public static class RGBAColorCollection
    {
        public static List<RGBAColor> Collection = new List<RGBAColor>() {
            new RGBAColor(0, 72, 186),
            new RGBAColor(175, 0, 42),
            new RGBAColor(59, 122, 87),
            new RGBAColor(176, 191, 26),
            new RGBAColor(114, 160, 193),
            new RGBAColor(255, 191, 0),
            new RGBAColor(255, 126, 0),
            new RGBAColor(241, 156, 187),
            new RGBAColor(132, 222, 2),
            new RGBAColor(195, 68, 122),
            new RGBAColor(127, 205, 205),
            new RGBAColor(225, 93, 68),
            new RGBAColor(85, 180, 176),
            new RGBAColor(223, 207, 190),
            new RGBAColor(155, 35, 53),
            new RGBAColor(91, 94, 166),
            new RGBAColor(239, 192, 80),
            new RGBAColor(68, 184, 172),
            new RGBAColor(214, 80, 118),
            new RGBAColor(221, 65, 36),
            new RGBAColor(0, 155, 119),
            new RGBAColor(181, 101, 167),
            new RGBAColor(149, 82, 81),
            new RGBAColor(146, 168, 209),
            new RGBAColor(247, 202, 201),
            new RGBAColor(136, 176, 75),
            new RGBAColor(107, 91, 149),
            new RGBAColor(255, 111, 97)
        };

        public static RGBAColor GenerateRandomRGB()
        {
            // Lower boundry of the random number generator is inclusive
            // Upper boundry of the random number generator is exclusive
            // Hence, range is: [0, 256) or [0, 255]
            return new RGBAColor(
                new Random().Next(0, 256),
                new Random().Next(0, 256),
                new Random().Next(0, 256)
                );
        }
    }
}
