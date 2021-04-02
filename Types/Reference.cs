using System;

namespace Kodefabrikken.Types
{
    /// <summary>
    /// A true reference type to avoid <see cref="NullReferenceException"/>'s in code.
    /// Throws <see cref="InvalidOperationException"/> on usage if initialized with default ctor.
    /// </summary>
    /// <typeparam name="T">The type of the referenced object.</typeparam>
    public struct Reference<T> where T : class
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
        /// Create a new <see cref="Reference{T}"/>.
        /// </summary>
        /// <param name="value">The reference object to reference.</param>
        /// <returns>The created reference.</returns>
        public static Reference<T> Create(T value) => new Reference<T>(value);

        /// <summary>
        /// Construction of a reference.
        /// </summary>
        /// <param name="value">The object to reference.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public Reference(T value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
            _properly_initialized = true;
        }

        /// <summary>
        /// Returns the referenced object.
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

            if(obj is Reference<T> v)
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
        public static bool operator ==(Reference<T> left, Reference<T> right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Not equal operator, check inequality of referenced objects.
        /// </summary>
        /// <param name="left">Left hand operator.</param>
        /// <param name="right">Right hand operator.</param>
        /// <returns></returns>
        public static bool operator !=(Reference<T> left, Reference<T> right)
        {
            return !(left == right);
        }
    }
}
