using System;

namespace Kodefabrikken.Types
{
    public static class OptionalExtensions
    {
        /// <summary>
        /// Converts <see cref="Nullable{T}"/> to <see cref="Optional{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of wrapped value, must be struct.</typeparam>
        /// <param name="value">Nullable object to convert.</param>
        /// <returns>Existing value wrapped in <see cref="Optional{T}"/>.</returns>
        public static Optional<T> ToOptional<T>(this Nullable<T> value) where T : struct
            => value.HasValue ? new Optional<T>(value.Value) : Optional<T>.Empty;

        /// <summary>
        /// Converts <see cref="Optional{T}"/> to <see cref="Nullable{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of wrapped value, must be struct.</typeparam>
        /// <param name="optional">Optional object to convert.</param>
        /// <returns>Existing value wrapped in <see cref="Nullable{T}"/>.</returns>
        public static Nullable<T> ToNullable<T>(this Optional<T> optional) where T : struct
        {
            Nullable<T> retval = null;
            optional.IfValue(value => retval = value);

            return retval;
        }

        // TODO : Should we remove this as we have implicit & Create ?
        /// <summary>
        /// Converts object to <see cref="Optional{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of wrapped value.</typeparam>
        /// <param name="value">Value to wrap.</param>
        /// <returns>Existing value wrapped in <see cref="Optional{T}"/>.</returns>
        public static Optional<T> ToOptional<T>(this T value)
            => value != null ? new Optional<T>(value) : Optional<T>.Empty;

        /// <summary>
        /// Convert <see cref="Optional{T}"/> to wrapped type.
        /// </summary>
        /// <typeparam name="T">Type of wrapped value, must be class.</typeparam>
        /// <param name="optional">Optional object to convert.</param>
        /// <returns>Existing value.</returns>
        public static T ToObject<T>(this Optional<T> optional) where T : class
        {
            T result = null;
            optional.IfValue(value => result = value);

            return result;
        }
    }
}