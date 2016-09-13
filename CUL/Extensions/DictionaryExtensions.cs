using System.Collections.Generic;

namespace Clapton.Extensions
{
    /// <summary>
    /// Provides extensions for Dictionaries
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Determines whether the <see cref="Dictionary{TKey, TValue}"/> contains all specified keys.
        /// </summary>
        /// <param name="keys">The keys to locate in <see cref="Dictionary{TKey, TValue}"/></param>
        public static bool ContainsKeys<TKey,TValue>(this Dictionary<TKey, TValue> obj, params TKey[] keys)
        {
            foreach (TKey key in keys)
                if (!obj.ContainsKey(key))
                    return false;
            return true;
        }

        /// <summary>
        /// Determines whether the <see cref="Dictionary{TKey, TValue}"/> contains any specified keys.
        /// </summary>
        /// <param name="keys">The keys to locate in <see cref="Dictionary{TKey, TValue}"/></param>
        public static bool ContainsAnyKey<TKey, TValue>(this Dictionary<TKey, TValue> obj, params TKey[] keys)
        {
            foreach (TKey key in keys)
                if (obj.ContainsKey(key))
                    return true;
            return false;
        }

        /// <summary>
        /// Determines whether the <see cref="Dictionary{TKey, TValue}"/> contains all specified values.
        /// </summary>
        /// <param name="values">The values to locate in <see cref="Dictionary{TKey, TValue}"/></param>
        public static bool ContainsValues<TKey,TValue>(this Dictionary<TKey, TValue> obj, params TValue[] values)
        {
            foreach (TValue value in values)
                if (!obj.ContainsValue(value))
                    return false;
            return true;
        }

        /// <summary>
        /// Determines whether the <see cref="Dictionary{TKey, TValue}"/> contains any specified values.
        /// </summary>
        /// <param name="values">The values to locate in <see cref="Dictionary{TKey, TValue}"/></param>
        public static bool ContainsAnyValue<TKey, TValue>(this Dictionary<TKey, TValue> obj, params TValue[] values)
        {
            foreach (TValue value in values)
                if (obj.ContainsValue(value))
                    return true;
            return false;
        }

        /// <summary>
        /// Determines whether a given <see cref="Dictionary{TKey, TValue}"/> contains a given key that also has a given value.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="Dictionary{TKey, TValue}"/>.</param>
        /// <param name="value">The value to match with the value stored in the given key.</param>
        public static bool ContainsKeyWithValue<TKey,TValue>(this Dictionary<TKey, TValue> obj, TKey key, TValue value)
        {
            return obj.ContainsKey(key) ? obj[key].Equals(value) : false;
        }

        /// <summary>
        /// Gets the value associated with the given key. Returns default value if key does not exist.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue GetValue<TKey,TValue>(this Dictionary<TKey,TValue> obj, TKey key)
        {
            return obj.ContainsKey(key) ? obj[key] : default(TValue);
        }
    }
}
