using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clapton.Extensions
{
    public static class ListExtensions
    {
        public static T GetAndRemoveAt<T>(this List<T> obj, int index)
        {
            T value = obj[index];
            obj.RemoveAt(index);
            return value;
        }
    }
}
