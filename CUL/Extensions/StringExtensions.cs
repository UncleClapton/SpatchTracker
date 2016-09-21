using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clapton.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Determined whether this <see cref="string"/> and a specified <see cref="string"/> object have the same value, ignoring the case of the strings being compared.
        /// </summary>
        /// <param name="obj">The selected <see cref="string"/> object</param>
        /// <param name="value">The <see cref="string"/> to compare to this instance</param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string obj, string value)
        {
            return obj.Equals(value, StringComparison.InvariantCultureIgnoreCase);
        }

        public static int? ToNullableInt(this string obj)
        {
            int value;
            if (int.TryParse(obj, out value)) return value;
            return null;
        }

        public static bool StartsWith(this string obj, params string[] values)
        {
            foreach (string value in values)
                if (obj.StartsWith(values)) return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string Slice(this string obj, int start, int end = int.MaxValue)
        {
            if (end == int.MaxValue)
                end = obj.Length;
            if (end < 0) // Keep this for negative end support
                end = obj.Length + end;
            int len = end - start;               // Calculate length
            return obj.Substring(start, len); // Return Substring of length
        }
    }
}
