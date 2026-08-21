namespace WarpCode.Outcomes;

/// <summary>
/// `OnValue` composition extensions for async outcomes that take an asynchronous delegate.
/// </summary>
/// <remarks>
/// Two families of overloads are provided: <c>OnValueAsync</c> for delegates returning a <see cref="System.Threading.Tasks.ValueTask"/> (the preferred, allocation-friendly form and the natural target for inline <c>async</c> lambdas) and <c>OnValueTask</c> for delegates returning a <see cref="System.Threading.Tasks.Task"/> (so an existing <c>Task</c>-returning method can be composed directly, including as a method group). The names are kept distinct rather than overloaded on return type because an <c>async</c> lambda is convertible to both, which would otherwise make every inline <c>async</c> lambda ambiguous (CS0121). A <see cref="CancellationToken"/> is supplied as the last delegate parameter.
/// </remarks>
public static class OnValueAsyncExtensions
{
    /// <param name="self">Current async outcome.</param>
    extension<T>(AsyncOutcome<T> self)
    {
        /// <summary>
        /// Wraps an async call to the `OnValue` operator on the underlying outcome.
        /// </summary>
        private async ValueTask<Outcome<T>> Local(Func<T, CancellationToken, ValueTask> onValue)
        {
            Outcome<T> outcome = await self;
            if (outcome.Problem is null)
                await onValue(outcome.Value, self.CancellationToken);
            return outcome;
        }

        /// <summary>
        /// Async happy-path composition operator.
        /// If the async outcome contains a valid value, the onValue action is executed.
        /// If the async outcome contains a problem, the onValue action is not executed and the problem is propagated.
        /// </summary>
        /// <param name="onValue">Action to execute if the outcome is successful.</param>
        /// <returns>The current outcome.</returns>
        public AsyncOutcome<T> OnValueAsync(Func<T, CancellationToken, ValueTask> onValue)
            => self.With(self.Local(onValue));

        /// <summary>
        /// `Task`-returning counterpart of <see cref="OnValueAsyncExtensions.extension{T}(AsyncOutcome{T}).OnValueAsync(Func{T, CancellationToken, ValueTask})"/>.
        /// Prefer the <c>OnValueAsync</c> (ValueTask) form for inline <c>async</c> lambdas; use this to compose an existing <c>Task</c>-returning method directly.
        /// </summary>
        /// <param name="onValue">Action to execute if the outcome is successful.</param>
        /// <returns>The current outcome.</returns>
        public AsyncOutcome<T> OnValueTask(Func<T, CancellationToken, Task> onValue)
            => self.OnValueAsync((v, c) => new ValueTask(onValue(v, c)));
    }

    /// <param name="self">Current value-less outcome.</param>
    extension(AsyncOutcome<None> self)
    {
        /// <inheritdoc cref="OnValueAsyncExtensions.extension{T}(AsyncOutcome{T}).OnValueAsync(Func{T, CancellationToken, ValueTask})"/>
        public AsyncOutcome<None> OnValueAsync(Func<CancellationToken, ValueTask> onValue)
            => self.OnValueAsync((_, c) => onValue(c));

        /// <inheritdoc cref="OnValueAsyncExtensions.extension{T}(AsyncOutcome{T}).OnValueTask(Func{T, CancellationToken, Task})"/>
        public AsyncOutcome<None> OnValueTask(Func<CancellationToken, Task> onValue)
            => self.OnValueAsync((_, c) => new ValueTask(onValue(c)));
    }

    /// <summary>OnValue extensions for an async outcome containing a tuple of two values.</summary>
    /// <param name="self">Current async outcome.</param>
    extension<T, T1>(AsyncOutcome<(T, T1)> self)
    {
        /// <inheritdoc cref="OnValueAsyncExtensions.extension{T}(AsyncOutcome{T}).OnValueAsync(Func{T, CancellationToken, ValueTask})"/>
        public AsyncOutcome<(T, T1)> OnValueAsync(Func<T, T1, CancellationToken, ValueTask> onValue)
            => self.OnValueAsync((v, c) => onValue(v.Item1, v.Item2, c));

        /// <inheritdoc cref="OnValueAsyncExtensions.extension{T}(AsyncOutcome{T}).OnValueTask(Func{T, CancellationToken, Task})"/>
        public AsyncOutcome<(T, T1)> OnValueTask(Func<T, T1, CancellationToken, Task> onValue)
            => self.OnValueTask((v, c) => onValue(v.Item1, v.Item2, c));
    }

    /// <summary>OnValue extensions for an async outcome containing a tuple of three values.</summary>
    /// <param name="self">Current async outcome.</param>
    extension<T, T1, T2>(AsyncOutcome<(T, T1, T2)> self)
    {
        /// <inheritdoc cref="OnValueAsyncExtensions.extension{T}(AsyncOutcome{T}).OnValueAsync(Func{T, CancellationToken, ValueTask})"/>
        public AsyncOutcome<(T, T1, T2)> OnValueAsync(Func<T, T1, T2, CancellationToken, ValueTask> onValue)
            => self.OnValueAsync((v, c) => onValue(v.Item1, v.Item2, v.Item3, c));

        /// <inheritdoc cref="OnValueAsyncExtensions.extension{T}(AsyncOutcome{T}).OnValueTask(Func{T, CancellationToken, Task})"/>
        public AsyncOutcome<(T, T1, T2)> OnValueTask(Func<T, T1, T2, CancellationToken, Task> onValue)
            => self.OnValueTask((v, c) => onValue(v.Item1, v.Item2, v.Item3, c));
    }

    /// <summary>OnValue extensions for an async outcome containing a tuple of four values.</summary>
    /// <param name="self">Current async outcome.</param>
    extension<T, T1, T2, T3>(AsyncOutcome<(T, T1, T2, T3)> self)
    {
        /// <inheritdoc cref="OnValueAsyncExtensions.extension{T}(AsyncOutcome{T}).OnValueAsync(Func{T, CancellationToken, ValueTask})"/>
        public AsyncOutcome<(T, T1, T2, T3)> OnValueAsync(Func<T, T1, T2, T3, CancellationToken, ValueTask> onValue)
            => self.OnValueAsync((v, c) => onValue(v.Item1, v.Item2, v.Item3, v.Item4, c));

        /// <inheritdoc cref="OnValueAsyncExtensions.extension{T}(AsyncOutcome{T}).OnValueTask(Func{T, CancellationToken, Task})"/>
        public AsyncOutcome<(T, T1, T2, T3)> OnValueTask(Func<T, T1, T2, T3, CancellationToken, Task> onValue)
            => self.OnValueTask((v, c) => onValue(v.Item1, v.Item2, v.Item3, v.Item4, c));
    }
}
