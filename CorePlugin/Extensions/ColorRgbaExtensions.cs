using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Drawing;

namespace DualityTiled.Extensions
{
    public static class ColorRgbaExtensions
    {
        public static string ToTiledHexString(this ColorRgba source)
        {
            return "#" + source.R.ToString("X") + source.G.ToString("X") + source.B.ToString("X");
        }
    }
}
