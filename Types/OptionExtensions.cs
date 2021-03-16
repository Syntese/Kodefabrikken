using System;

namespace Kodefabrikken.Types
{
    public static class OptionExtensions
    {
        /// <summary>
        /// Converts <see cref="Nullable{T}"/> to <see cref="Option{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of wrapped value, must be struct.</typeparam>
        /// <param name="value">Nullable object to convert.</param>
        /// <returns>Existing value wrapped in <see cref="Option{T}"/>.</returns>
        public static Option<T> ToOption<T>(this Nullable<T> value) where T : struct
            => value.HasValue ? new Option<T>(value.Value) : Option<T>.Empty;

        /// <summary>
        /// Converts <see cref="Option{T}"/> to <see cref="Nullable{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of wrapped value, must be struct.</typeparam>
        /// <param name="option">Option object to convert.</param>
        /// <returns>Existing value wrapped in <see cref="Nullable{T}"/>.</returns>
        public static Nullable<T> ToNullable<T>(this Option<T> option) where T : struct
        {
            Nullable<T> retval = null;
            option.IfValue(value => retval = value);

            return retval;
        }

        /// <summary>
        /// Converts object to <see cref="Option{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of wrapped value.</typeparam>
        /// <param name="value">Value to wrap.</param>
        /// <returns>Existing value wrapped in <see cref="Option{T}"/>.</returns>
        public static Option<T> ToOption<T>(this T value)
            => value != null ? new Option<T>(value) : Option<T>.Empty;

        /// <summary>
        /// Convert <see cref="Option{T}"/> to wrapped type.
        /// </summary>
        /// <typeparam name="T">Type of wrapped value, must be class.</typeparam>
        /// <param name="option">Option object to convert.</param>
        /// <returns>Existing value.</returns>
        public static T ToObject<T>(this Option<T> option) where T : class
        {
            T result = null;
            option.IfValue(value => result = value);

            return result;
        }
    }
}