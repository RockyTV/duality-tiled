using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DualityTiled.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Capitalizes/uppercases the first letter of the specified string.
        /// </summary>
        public static string CapitalizeFirstLetter(this string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                return char.ToUpperInvariant(source[0]) + source.Substring(1);
            }
            return null;
        }
    }
}
