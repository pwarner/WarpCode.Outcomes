namespace WarpCode.Outcomes;

/// <summary>
/// `Ensure` composition extensions for async outcomes.
/// </summary>
public static class AsyncEnsureExtensions
{
    /// <param name="self">Current async outcome.</param>
    extension<T>(AsyncOutcome<T> self)
    {
        /// <summary>
        /// Wraps an async call to the `Ensure` extension on the underlying outcome.
        /// </summary>
        private async ValueTask<Outcome<T>> Local(Func<T, Outcome<None>> ensure)
            => (await self).Ensure(ensure);

        /// <summary>
        /// Async happy-path composition operator.
        /// If the async outcome resolves to a valid value, the ensure function is executed to validate the value.
        /// If the async outcome resolves to a problem, the ensure function is not executed and the problem is propagated.
        /// </summary>
        /// <remarks>
        /// Represents a function that validates the outcome value against defined criteria and returns an outcome indicating whether the validation succeeded or failed.
        /// </remarks>
        /// <param name="ensure">Function to execute for validation.</param>
        /// <returns>The current async outcome if it was valid, otherwise a new async outcome representing the problem.</returns>
        public AsyncOutcome<T> Ensure(Func<T, Outcome<None>> ensure)
            => self.With(self.Local(ensure));
    }

    /// <summary>Ensure extensions for an async outcome containing a tuple of two values.</summary>
    /// <param name="self">Current async outcome.</param>
    extension<T, T1>(AsyncOutcome<(T, T1)> self)
    {
        /// <inheritdoc cref="EnsureExtensions.extension{T}(Outcome{T}).Ensure(Func{T, Outcome{None}})"/>
        public AsyncOutcome<(T, T1)> Ensure(Func<T, T1, Outcome<None>> ensure)
            => self.Ensure(value => ensure(value.Item1, value.Item2));
    }

    /// <summary>Ensure extensions for an async outcome containing a tuple of three values.</summary>
    /// <param name="self">Current async outcome.</param>
    extension<T, T1, T2>(AsyncOutcome<(T, T1, T2)> self)
    {
        /// <inheritdoc cref="EnsureExtensions.extension{T}(Outcome{T}).Ensure(Func{T, Outcome{None}})"/>
        public AsyncOutcome<(T, T1, T2)> Ensure(Func<T, T1, T2, Outcome<None>> ensure)
            => self.Ensure(value => ensure(value.Item1, value.Item2, value.Item3));
    }


    /// <summary>Ensure extensions for an async outcome containing a tuple of four values.</summary>
    /// <param name="self">Current async outcome.</param>
    extension<T, T1, T2, T3>(AsyncOutcome<(T, T1, T2, T3)> self)
    {
        /// <inheritdoc cref="EnsureExtensions.extension{T}(Outcome{T}).Ensure(Func{T, Outcome{None}})"/>
        public AsyncOutcome<(T, T1, T2, T3)> Ensure(Func<T, T1, T2, T3, Outcome<None>> ensure)
            => self.Ensure(value => ensure(value.Item1, value.Item2, value.Item3, value.Item4));
    }
}