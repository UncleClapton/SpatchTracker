using System.Collections.Generic;

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
