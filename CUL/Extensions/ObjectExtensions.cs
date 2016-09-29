using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clapton.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Determines whether the given <see cref="object"/> matches any specified <see cref="object"/>.
        /// </summary>
        /// <param name="obj">The selected <see cref="object"/></param>
        /// <param name="value">The <see cref="object"/> to compare to the given object</param>
        /// <returns></returns>
        public static bool Equals(this object obj, params object[] values)
        {
            foreach (string value in values)
                if (obj.Equals(value)) return true;
            return false;
        }

    }
}
