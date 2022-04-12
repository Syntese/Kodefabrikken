using System;

namespace Kodefabrikken.Types
{
    /// <summary>
    /// A type for handling optional values without any chance of <see cref="NullReferenceException"/>.
    /// <see cref="Nullable{T}"/> and <see cref="Optional{T}"/> can't be optional values.
    /// </summary>
    /// <typeparam name="T">Type of the optional value.</typeparam>
    public struct Optional<T>
    {
        static readonly Type gOptionalType = typeof(T);
        static readonly bool gIsNullableOptionalType = gOptionalType.IsGenericType && gOptionalType.GetGenericTypeDefinition() == typeof(Nullable<>);
        static readonly bool gIsOptionalOptionalType = gOptionalType.IsGenericType && gOptionalType.GetGenericTypeDefinition() == typeof(Optional<>);

        /// <summary>
        /// Type of the optional value.
        /// </summary>
        public static Type OptionType
        {
            get
            {
                if (gIsNullableOptionalType || gIsOptionalOptionalType)
                {
                    throw new InvalidOperationException();
                }

                return gOptionalType;
            }
        }

        static readonly Optional<T> _empty = default;

        /// <summary>
        /// An optional without a value.
        /// </summary>
        public static Optional<T> Empty
        {
            get
            {
                if (gIsNullableOptionalType || gIsOptionalOptionalType)
                {
                    throw new InvalidOperationException();
                }

                return _empty;
            }
        }

        /// <summary>
        /// Creates an empty optional.
        /// </summary>
        /// <returns>The empty optional.</returns>
        public static Optional<T> Create() => Empty;

        /// <summary>
        /// Creates an optional.
        /// </summary>
        /// <param name="value">The value of the optional.</param>
        /// <returns>The created optional. <see cref="Empty"/> if <paramref name="value"/> is null.</returns>
        public static Optional<T> Create(T value) => value != null ? new Optional<T>(value) : Empty;

        /// <summary>
        /// Implicitly convert any value to <see cref="Optional{T}"/>.
        /// </summary>
        /// <param name="value">The value of the optional.</param>
        public static implicit operator Optional<T>(T value) => Create(value);

        T Value { get; }

        /// <summary>
        /// Creates an <see cref="Optional{T}"/>, a non-mutable type with a value.
        /// Use <see cref="Optional{T}.Empty"/> for empty optionals.
        /// </summary>
        /// <param name="value">The value of the optional.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is Nullable or Optional.</exception>
        public Optional(T value)
        {
            if (gIsNullableOptionalType)
            {
                throw new ArgumentException($"{nameof(value)} is Nullable, use ToOptional() instead.");
            }

            if (gIsOptionalOptionalType)
            {
                throw new ArgumentException($"{nameof(value)} is Optional");
            }

            // TODO : Should this create an empty optional?
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
            HasValue = true;
        }

        /// <summary>
        /// true if <see cref="Optional{T}"/> has value.
        /// </summary>
        public bool HasValue { get; }

        class IfValueContext : IIfContext
        {
            readonly Optional<T> _optional;

            internal IfValueContext(Optional<T> optional)
            {
                _optional = optional;
            }

            /// <inheritdoc/>
            public void Else(Action action)
            {
                if (!_optional.HasValue)
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
        /// Return optional value if <see cref="HasValue"/>, supplied <paramref name="value"/> otherwise.
        /// </summary>
        /// <param name="value">Value to return if <see cref="HasValue"/> is false.</param>
        /// <returns>The coalesced value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null, evaluated even if <see cref="HasValue"/>.</exception>
        public T Coalesce(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return HasValue ? Value : value;
        }

        /// <summary>
        /// Returns optional value if <see cref="HasValue"/>, value from <paramref name="value_func"/> otherwise.
        /// </summary>
        /// <param name="value_func">Function for alternate value if <see cref="HasValue"/> is false.</param>
        /// <returns>The coalsced value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value_func"/> is null, evaluated even if <see cref="HasValue"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="value_func"/> evaluates to null, only evaluated if <see cref="HasValue"/> is false.</exception>
        public T Coalesce(Func<T> value_func)
        {
            T result;

            if (value_func == null)
            {
                throw new ArgumentNullException(nameof(value_func));
            }

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

        /// <summary>
        /// Cast the <see cref="Optional{T}"/> to another object.
        /// </summary>
        /// <typeparam name="U">Type of the new object.</typeparam>
        /// <param name="fromValue">Function to use when casting optional with value.</param>
        /// <param name="fromEmpty">Function to use when casting empty optional.</param>
        /// <returns></returns>
        public U Cast<U>(Func<T, U> fromValue, Func<U> fromEmpty) => HasValue ? fromValue(Value) : fromEmpty();

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return !HasValue;
            }

            if (obj is Optional<T> val)
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
        public static bool operator ==(Optional<T> left, Optional<T> right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Not equal operator.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>true if operands are non-equal.</returns>
        public static bool operator !=(Optional<T> left, Optional<T> right)
        {
            return !(left == right);
        }
    }
}