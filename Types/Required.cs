using System;

namespace Kodefabrikken.Types
{
    /// <summary>
    /// A true reference type to avoid <see cref="NullReferenceException"/>'s in code.
    /// Throws <see cref="InvalidOperationException"/> on usage if initialized with default ctor.
    /// </summary>
    /// <typeparam name="T">The type of the required object.</typeparam>
    public struct Required<T> where T : class
    {
        readonly bool _properly_initialized;
        readonly T _value;

        void UsageGuard()
        {
            if (!_properly_initialized)
            {
                throw new InvalidOperationException("Don't initialize with default ctor.");
            }
        }

        /// <summary>
        /// Create a new <see cref="Required{T}"/>.
        /// </summary>
        /// <param name="value">The reference object to make required.</param>
        /// <returns>The created required.</returns>
        public static Required<T> Create(T value) => new Required<T>(value);

        /// <summary>
        /// Construction of a required reference.
        /// </summary>
        /// <param name="value">The object to reference.</param>
        /// <exception cref="ArgumentException"><paramref name="value"/> is of type Required</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public Required(T value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
            _properly_initialized = true;
        }

        /// <summary>
        /// Returns the required object.
        /// </summary>
        public T Value
        {
            get
            {
                UsageGuard();

                return _value;
            }
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            UsageGuard();

            if (obj is Required<T> v)
            {
                return _value.Equals(v._value);
            }

            return _value.Equals(obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            UsageGuard();

            return _value.GetHashCode();
        }

        /// <summary>
        /// Equal operator, check equality of referenced objects.
        /// </summary>
        /// <param name="left">The left hand operand.</param>
        /// <param name="right">The right hand operand.</param>
        /// <returns></returns>
        public static bool operator ==(Required<T> left, Required<T> right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Not equal operator, check inequality of referenced objects.
        /// </summary>
        /// <param name="left">Left hand operator.</param>
        /// <param name="right">Right hand operator.</param>
        /// <returns></returns>
        public static bool operator !=(Required<T> left, Required<T> right)
        {
            return !(left == right);
        }
    }
}
