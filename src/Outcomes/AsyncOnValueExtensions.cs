namespace WarpCode.Outcomes;

/// <summary>
/// `OnValue` composition extensions for async outcomes
/// </summary>
public static class AsyncOnValueExtensions
{
    /// <param name="self">Current async outcome.</param>
    extension<T>(AsyncOutcome<T> self)
    {
        /// <summary>
        /// Wraps an async call to the `OnValue` extension on the underlying outcome.
        /// </summary>
        private async ValueTask<Outcome<T>> Local(Action<T> onValue)
            => (await self).OnValue(onValue);

        /// <summary>
        /// Async happy-path composition operator.
        /// If the async outcome contains a valid value, the onValue action is executed.
        /// If the async outcome contains a problem, the onValue action is not executed and the problem is propagated.
        /// </summary>
        /// <param name="onValue">Action to execute if the outcome is successful.</param>
        /// <returns>The current outcome.</returns>
        public AsyncOutcome<T> OnValue(Action<T> onValue)
            => self.With(self.Local(onValue));  
    }

    /// <param name="self">Current value-less outcome.</param>
    extension(AsyncOutcome<None> self)
    {
        /// <inheritdoc cref="AsyncOnValueExtensions.extension{T}(AsyncOutcome{T}).OnValue(Action{T})"/>
        public AsyncOutcome<None> OnValue(Action onValue)
            => self.OnValue(_=> onValue());
    }

    /// <summary>OnValue extensions for an async outcome containing a tuple of two values.</summary>
    /// <param name="self">Current async outcome.</param>
    extension<T, T1>(AsyncOutcome<(T, T1)> self)
    {
        /// <inheritdoc cref="AsyncOnValueExtensions.extension{T}(AsyncOutcome{T}).OnValue(Action{T})"/>
        public AsyncOutcome<(T, T1)> OnValue(Action<T, T1> onValue)
            => self.OnValue(value => onValue(value.Item1, value.Item2));
    }

    /// <summary>OnValue extensions for an async outcome containing a tuple of three values.</summary>
    /// <param name="self">Current async outcome.</param>
    extension<T, T1, T2>(AsyncOutcome<(T, T1, T2)> self)
    {
        /// <inheritdoc cref="AsyncOnValueExtensions.extension{T}(AsyncOutcome{T}).OnValue(Action{T})"/>
        public AsyncOutcome<(T, T1, T2)> OnValue(Action<T, T1, T2> onValue)
            => self.OnValue(value => onValue(value.Item1, value.Item2, value.Item3));
    }

    /// <summary>OnValue extensions for an async outcome containing a tuple of four values.</summary>
    /// <param name="self">Current async outcome.</param>
    extension<T, T1, T2, T3>(AsyncOutcome<(T, T1, T2, T3)> self)
    {
        /// <inheritdoc cref="AsyncOnValueExtensions.extension{T}(AsyncOutcome{T}).OnValue(Action{T})"/>
        public AsyncOutcome<(T, T1, T2, T3)> OnValue(Action<T, T1, T2, T3> onValue)
            => self.OnValue(value => onValue(value.Item1, value.Item2, value.Item3, value.Item4));
    }
}