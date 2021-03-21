using System;

namespace Kodefabrikken.Types
{
    /// <summary>
    /// A type for handling optional values without any chance of <see cref="NullReferenceException"/>.
    /// <see cref="Nullable{T}"/> and <see cref="Option{T}"/> can't be optional values.
    /// </summary>
    /// <typeparam name="T">Type of the optional value.</typeparam>
    public struct Option<T>
    {
        static readonly Type gOptionType = typeof(T);
        static readonly bool gIsNullableOptionType = gOptionType.IsGenericType && gOptionType.GetGenericTypeDefinition() == typeof(Nullable<>);
        static readonly bool gIsOptionOptionType = gOptionType.IsGenericType && gOptionType.GetGenericTypeDefinition() == typeof(Option<>);

        /// <summary>
        /// Type of the optional value.
        /// </summary>
        public static Type OptionType
        {
            get
            {
                if (gIsNullableOptionType || gIsOptionOptionType)
                {
                    throw new InvalidOperationException();
                }

                return gOptionType;
            }
        }

        static readonly Option<T> _empty = default;

        /// <summary>
        /// An option without a value.
        /// </summary>
        public static Option<T> Empty
        {
            get
            {
                if (gIsNullableOptionType || gIsOptionOptionType)
                {
                    throw new InvalidOperationException();
                }

                return _empty;
            }
        }

        T Value { get; }

        /// <summary>
        /// Creates an <see cref="Option{T}"/>, a non-mutable type with a value.
        /// Use <see cref="Option{T}.Empty"/> for empty options.
        /// </summary>
        /// <param name="value">The value of the option.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is Nullable or Option.</exception>
        public Option(T value)
        {
            if (gIsNullableOptionType)
            {
                throw new ArgumentException($"{nameof(value)} is Nullable, use ToOption() instead.");
            }

            if (gIsOptionOptionType)
            {
                throw new ArgumentException($"{nameof(value)} is Option");
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
            HasValue = true;
        }

        /// <summary>
        /// true if <see cref="Option{T}"/> has value.
        /// </summary>
        public bool HasValue { get; }

        class IfValueContext : IIfContext
        {
            readonly Option<T> _option;

            internal IfValueContext(Option<T> option)
            {
                _option = option;
            }

            /// <inheritdoc/>
            public void Else(Action action)
            {
                if (!_option.HasValue)
                {
                    action();
                };
            }
        }

        /// <summary>
        /// Register action to run if <see cref="HasValue"/>.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        /// <returns>Context for no value action.</returns>
        public IIfContext IfValue(Action<T> action)
        {
            if (HasValue)
            {
                action(Value);
            }

            return new IfValueContext(this);
        }

        /// <summary>
        /// Return option value if <see cref="HasValue"/>, supplied <paramref name="value"/> otherwise.
        /// </summary>
        /// <param name="value">Value to return if <see cref="HasValue"/> is false.</param>
        /// <returns>The coalesced value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null, evaluated even if <see cref="HasValue"/>.</exception>
        public T Coalesce(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            return HasValue ? Value : value;
        }

        /// <summary>
        /// Returns option value if <see cref="HasValue"/>, value from <paramref name="value_func"/> otherwise.
        /// </summary>
        /// <param name="value_func">Function for alternate value if <see cref="HasValue"/> is false.</param>
        /// <returns>The coalsced value.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="value_func"/> evaluates to null, only evaluated if <see cref="HasValue"/> is false.</exception>
        public T Coalesce(Func<T> value_func)
        {
            T result;

            if (HasValue)
            {
                result = Value;
            }
            else
            {
                result = value_func();
                if (result == null)
                {
                    throw new InvalidOperationException();
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return !HasValue;
            }

            if (obj is Option<T> val)
            {
                if (!val.HasValue)
                {
                    return !HasValue;
                }

                if (HasValue)
                {
                    return Value.Equals(val.Value);
                }
            }

            if (!HasValue)
            {
                return false;
            }

            return Value.Equals(obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
            => HasValue ? Value.GetHashCode() : 0;

        /// <summary>
        /// Equal operator.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>true if operands are equal.</returns>
        public static bool operator ==(Option<T> left, Option<T> right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Not equal operator.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>true if operands are non-equal.</returns>
        public static bool operator !=(Option<T> left, Option<T> right)
        {
            return !(left == right);
        }
    }
}