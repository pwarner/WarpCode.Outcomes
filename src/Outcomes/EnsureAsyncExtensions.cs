namespace WarpCode.Outcomes;

/// <summary>
/// `Ensure` composition extensions for async outcomes that take an asynchronous delegate.
/// </summary>
/// <remarks>
/// Two families of overloads are provided: <c>EnsureAsync</c> for delegates returning a <see cref="System.Threading.Tasks.ValueTask{TResult}"/> (the preferred, allocation-friendly form and the natural target for inline <c>async</c> lambdas) and <c>EnsureTask</c> for delegates returning a <see cref="System.Threading.Tasks.Task{TResult}"/> (so an existing <c>Task</c>-returning method can be composed directly, including as a method group). The names are kept distinct rather than overloaded on return type because an <c>async</c> lambda is convertible to both, which would otherwise make every inline <c>async</c> lambda ambiguous (CS0121). A <see cref="CancellationToken"/> is supplied as the last delegate parameter.
/// </remarks>
public static class EnsureAsyncExtensions
{
    /// <param name="self">Current async outcome.</param>
    extension<T>(AsyncOutcome<T> self)
    {
        /// <summary>
        /// Wraps an async call to the `Ensure` operator on the underlying outcome.
        /// </summary>
        private async ValueTask<Outcome<T>> Local(Func<T, CancellationToken, ValueTask<Outcome<None>>> ensure)
            => (await self) switch
            {
                { Problem: not null } outcome => outcome,
                var outcome => (await ensure(outcome.Value, self.CancellationToken)).Then(outcome.Value)
            };

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
        public AsyncOutcome<T> EnsureAsync(Func<T, CancellationToken, ValueTask<Outcome<None>>> ensure)
            => self.With(self.Local(ensure));

        /// <summary>
        /// `Task`-returning counterpart of <see cref="EnsureAsyncExtensions.extension{T}(AsyncOutcome{T}).EnsureAsync(Func{T, CancellationToken, ValueTask{Outcome{None}}})"/>.
        /// Prefer the <c>EnsureAsync</c> (ValueTask) form for inline <c>async</c> lambdas; use this to compose an existing <c>Task</c>-returning method directly.
        /// </summary>
        /// <param name="ensure">Function to execute for validation.</param>
        /// <returns>The current async outcome if it was valid, otherwise a new async outcome representing the problem.</returns>
        public AsyncOutcome<T> EnsureTask(Func<T, CancellationToken, Task<Outcome<None>>> ensure)
            => self.EnsureAsync((v, c) => new ValueTask<Outcome<None>>(ensure(v, c)));
    }

    /// <summary>Ensure extensions for an async outcome containing no value.</summary>
    /// <param name="self">Current value-less async outcome.</param>
    extension(AsyncOutcome<None> self)
    {
        /// <inheritdoc cref="EnsureAsyncExtensions.extension{T}(AsyncOutcome{T}).EnsureAsync(Func{T, CancellationToken, ValueTask{Outcome{None}}})"/>
        public AsyncOutcome<None> EnsureAsync(Func<CancellationToken, ValueTask<Outcome<None>>> ensure)
            => self.EnsureAsync((_, c) => ensure(c));

        /// <inheritdoc cref="EnsureAsyncExtensions.extension{T}(AsyncOutcome{T}).EnsureTask(Func{T, CancellationToken, Task{Outcome{None}}})"/>
        public AsyncOutcome<None> EnsureTask(Func<CancellationToken, Task<Outcome<None>>> ensure)
            => self.EnsureAsync((_, c) => new ValueTask<Outcome<None>>(ensure(c)));
    }

    /// <summary>Ensure extensions for an async outcome containing a tuple of two values.</summary>
    /// <param name="self">Current async outcome.</param>
    extension<T, T1>(AsyncOutcome<(T, T1)> self)
    {
        /// <inheritdoc cref="EnsureAsyncExtensions.extension{T}(AsyncOutcome{T}).EnsureAsync(Func{T, CancellationToken, ValueTask{Outcome{None}}})"/>
        public AsyncOutcome<(T, T1)> EnsureAsync(Func<T, T1, CancellationToken, ValueTask<Outcome<None>>> ensure)
            => self.EnsureAsync((v, c) => ensure(v.Item1, v.Item2, c));

        /// <inheritdoc cref="EnsureAsyncExtensions.extension{T}(AsyncOutcome{T}).EnsureTask(Func{T, CancellationToken, Task{Outcome{None}}})"/>
        public AsyncOutcome<(T, T1)> EnsureTask(Func<T, T1, CancellationToken, Task<Outcome<None>>> ensure)
            => self.EnsureTask((v, c) => ensure(v.Item1, v.Item2, c));
    }

    /// <summary>Ensure extensions for an async outcome containing a tuple of three values.</summary>
    /// <param name="self">Current async outcome.</param>
    extension<T, T1, T2>(AsyncOutcome<(T, T1, T2)> self)
    {
        /// <inheritdoc cref="EnsureAsyncExtensions.extension{T}(AsyncOutcome{T}).EnsureAsync(Func{T, CancellationToken, ValueTask{Outcome{None}}})"/>
        public AsyncOutcome<(T, T1, T2)> EnsureAsync(Func<T, T1, T2, CancellationToken, ValueTask<Outcome<None>>> ensure)
            => self.EnsureAsync((v, c) => ensure(v.Item1, v.Item2, v.Item3, c));

        /// <inheritdoc cref="EnsureAsyncExtensions.extension{T}(AsyncOutcome{T}).EnsureTask(Func{T, CancellationToken, Task{Outcome{None}}})"/>
        public AsyncOutcome<(T, T1, T2)> EnsureTask(Func<T, T1, T2, CancellationToken, Task<Outcome<None>>> ensure)
            => self.EnsureTask((v, c) => ensure(v.Item1, v.Item2, v.Item3, c));
    }

    /// <summary>Ensure extensions for an async outcome containing a tuple of four values.</summary>
    /// <param name="self">Current async outcome.</param>
    extension<T, T1, T2, T3>(AsyncOutcome<(T, T1, T2, T3)> self)
    {
        /// <inheritdoc cref="EnsureAsyncExtensions.extension{T}(AsyncOutcome{T}).EnsureAsync(Func{T, CancellationToken, ValueTask{Outcome{None}}})"/>
        public AsyncOutcome<(T, T1, T2, T3)> EnsureAsync(Func<T, T1, T2, T3, CancellationToken, ValueTask<Outcome<None>>> ensure)
            => self.EnsureAsync((v, c) => ensure(v.Item1, v.Item2, v.Item3, v.Item4, c));

        /// <inheritdoc cref="EnsureAsyncExtensions.extension{T}(AsyncOutcome{T}).EnsureTask(Func{T, CancellationToken, Task{Outcome{None}}})"/>
        public AsyncOutcome<(T, T1, T2, T3)> EnsureTask(Func<T, T1, T2, T3, CancellationToken, Task<Outcome<None>>> ensure)
            => self.EnsureTask((v, c) => ensure(v.Item1, v.Item2, v.Item3, v.Item4, c));
    }
}
