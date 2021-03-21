using System;

namespace Kodefabrikken.Types
{
    /// <summary>
    /// Interface for the context after an <see cref="Option{T}.IfValue(Action{T})"/>.
    /// </summary>
    public interface IIfContext
    {
        /// <summary>
        /// Register action run if <see cref="Option{T}.HasValue"/> is false.
        /// </summary>
        /// <param name="action">Action run if <see cref="Option{T}"/> has no value.</param>
        void Else(Action action);
    }
}