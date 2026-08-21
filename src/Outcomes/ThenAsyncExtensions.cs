namespace WarpCode.Outcomes;

/// <summary>
/// `Then` composition extensions for async outcomes that take an asynchronous delegate.
/// </summary>
/// <remarks>
/// Two families of overloads are provided for delegates that return an awaitable:
/// <list type="bullet">
/// <item><description><c>ThenAsync</c> — the delegate returns a <see cref="System.Threading.Tasks.ValueTask"/> / <see cref="System.Threading.Tasks.ValueTask{TResult}"/>. This is the preferred, allocation-friendly form and the natural target for inline <c>async</c> lambdas.</description></item>
/// <item><description><c>ThenTask</c> — the delegate returns a <see cref="System.Threading.Tasks.Task"/> / <see cref="System.Threading.Tasks.Task{TResult}"/>. Use this when composing an existing <c>Task</c>-returning method, so it can be passed directly (including as a method group) without wrapping it in a <c>ValueTask</c>.</description></item>
/// </list>
/// The two families are deliberately given distinct names rather than overloaded on return type: an <c>async</c> lambda is convertible to both <c>Task</c> and <c>ValueTask</c>, so a single overloaded name would make every inline <c>async</c> lambda ambiguous (CS0121). In all cases a <see cref="CancellationToken"/> is supplied as the last delegate parameter.
/// </remarks>
public static class ThenAsyncExtensions
{
    /// <param name="self">Current async outcome.</param>
    extension<T, TNext>(AsyncOutcome<T> self)
    {
        /// <summary>
        /// Wraps an async call to the `Then` extension on the underlying outcome.
        /// </summary>
        private async ValueTask<Outcome<TNext>> Local(Func<T, CancellationToken, ValueTask<Outcome<TNext>>> next)
            => (await self) switch
            {
                { Problem: { } problem } => new Outcome<TNext>(problem),
                var outcome => await next(outcome.Value, self.CancellationToken)
            };

        /// <summary>
        /// Async happy-path composition operator.
        /// If the async outcome resolves to a valid value, the `next` function is executed to produce the next outcome.
        /// If the async outcome resolves to a problem, the `next` function is not executed and the problem is propagated.
        /// </summary>
        /// <param name="next">Function to execute to obtain next outcome.</param>
        /// <returns>An AsyncOutcome{TNext} representing the result of the composition.</returns>
        public AsyncOutcome<TNext> ThenAsync(Func<T, CancellationToken, ValueTask<Outcome<TNext>>> next)
            => self.With(self.Local(next));

        /// <summary>
        /// Async happy-path composition operator.
        /// If the async outcome resolves to a valid value, the `next` function is executed to produce the next outcome.
        /// If the async outcome resolves to a problem, the `next` function is not executed and the problem is propagated.
        /// </summary>
        /// <param name="next">Function to execute to obtain next value.</param>
        /// <returns>An AsyncOutcome{TNext} representing the result of the composition.</returns>
        public AsyncOutcome<TNext> ThenAsync(Func<T, CancellationToken, ValueTask<TNext>> next)
            => self.With(self.Local<T, TNext>(async (v, c)=> new(await next(v, c))));

        /// <summary>
        /// `Task`-returning counterpart of <see cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{Outcome{TNext}}})"/>.
        /// Prefer the <c>ThenAsync</c> (ValueTask) form for inline <c>async</c> lambdas; use this to compose an existing <c>Task</c>-returning method directly.
        /// </summary>
        /// <param name="next">Function to execute to obtain next outcome.</param>
        /// <returns>An AsyncOutcome{TNext} representing the result of the composition.</returns>
        public AsyncOutcome<TNext> ThenTask(Func<T, CancellationToken, Task<Outcome<TNext>>> next)
            => self.ThenAsync((v, c) => new ValueTask<Outcome<TNext>>(next(v, c)));

        /// <summary>
        /// `Task`-returning counterpart of <see cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{TNext}})"/>.
        /// Prefer the <c>ThenAsync</c> (ValueTask) form for inline <c>async</c> lambdas; use this to compose an existing <c>Task</c>-returning method directly.
        /// </summary>
        /// <param name="next">Function to execute to obtain next value.</param>
        /// <returns>An AsyncOutcome{TNext} representing the result of the composition.</returns>
        public AsyncOutcome<TNext> ThenTask(Func<T, CancellationToken, Task<TNext>> next)
            => self.ThenAsync((v, c) => new ValueTask<TNext>(next(v, c)));
    }

    /// <summary>Then extensions for an outcome containing no value.</summary>
    /// <param name="self">Current value-less outcome.</param>
    extension<TNext>(AsyncOutcome<None> self)
    {
        /// <inheritdoc cref=" ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{Outcome{TNext}}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<CancellationToken, ValueTask<Outcome<TNext>>> next)
            => self.ThenAsync((_, c) => next(c));

        /// <inheritdoc cref=" ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{TNext}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<CancellationToken, ValueTask<TNext>> next)
            => self.ThenAsync((_, c) => next(c));

        /// <inheritdoc cref=" ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenTask(Func{T, CancellationToken, Task{Outcome{TNext}}})"/>
        public AsyncOutcome<TNext> ThenTask(Func<CancellationToken, Task<Outcome<TNext>>> next)
            => self.ThenAsync((_, c) => new ValueTask<Outcome<TNext>>(next(c)));

        /// <inheritdoc cref=" ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenTask(Func{T, CancellationToken, Task{TNext}})"/>
        public AsyncOutcome<TNext> ThenTask(Func<CancellationToken, Task<TNext>> next)
            => self.ThenAsync((_, c) => new ValueTask<TNext>(next(c)));
    }

    /// <summary>Then extensions for an async outcome containing a tuple of two values.</summary>
    /// <param name="self">Current async outcome.</param>
    extension<T, T1, TNext>(AsyncOutcome<(T, T1)> self)
    {
        /// <inheritdoc cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{Outcome{TNext}}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<T, T1, CancellationToken, ValueTask<Outcome<TNext>>> next)
            => self.ThenAsync((v, c) => next(v.Item1, v.Item2, c));

        /// <inheritdoc cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{TNext}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<T, T1, CancellationToken, ValueTask<TNext>> next)
            => self.ThenAsync((v, c) => next(v.Item1, v.Item2, c));

        /// <inheritdoc cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenTask(Func{T, CancellationToken, Task{Outcome{TNext}}})"/>
        public AsyncOutcome<TNext> ThenTask(Func<T, T1, CancellationToken, Task<Outcome<TNext>>> next)
            => self.ThenTask((v, c) => next(v.Item1, v.Item2, c));

        /// <inheritdoc cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenTask(Func{T, CancellationToken, Task{TNext}})"/>
        public AsyncOutcome<TNext> ThenTask(Func<T, T1, CancellationToken, Task<TNext>> next)
            => self.ThenTask((v, c) => next(v.Item1, v.Item2, c));
    }

    /// <summary>Then extensions for an async outcome containing a tuple of three values.</summary>
    /// <param name="self">Current async outcome.</param>
    extension<T, T1, T2, TNext>(AsyncOutcome<(T, T1, T2)> self)
    {
        /// <inheritdoc cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{Outcome{TNext}}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<T, T1, T2, CancellationToken, ValueTask<Outcome<TNext>>> next)
            => self.ThenAsync((v,c) => next(v.Item1, v.Item2, v.Item3, c));

        /// <inheritdoc cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{TNext}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<T, T1, T2, CancellationToken, ValueTask<TNext>> next)
            => self.ThenAsync((v,c) => next(v.Item1, v.Item2, v.Item3, c));

        /// <inheritdoc cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenTask(Func{T, CancellationToken, Task{Outcome{TNext}}})"/>
        public AsyncOutcome<TNext> ThenTask(Func<T, T1, T2, CancellationToken, Task<Outcome<TNext>>> next)
            => self.ThenTask((v, c) => next(v.Item1, v.Item2, v.Item3, c));

        /// <inheritdoc cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenTask(Func{T, CancellationToken, Task{TNext}})"/>
        public AsyncOutcome<TNext> ThenTask(Func<T, T1, T2, CancellationToken, Task<TNext>> next)
            => self.ThenTask((v, c) => next(v.Item1, v.Item2, v.Item3, c));
    }

    /// <summary>Then extensions for an async outcome containing a tuple of four values.</summary>
    /// <param name="self">Current async outcome.</param>
    extension<T, T1, T2, T3, TNext>(AsyncOutcome<(T, T1, T2, T3)> self)
    {
        /// <inheritdoc cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{Outcome{TNext}}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<T, T1, T2, T3, CancellationToken, ValueTask<Outcome<TNext>>> next)
            => self.ThenAsync((v,c) => next(v.Item1, v.Item2, v.Item3, v.Item4, c));

        /// <inheritdoc cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{TNext}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<T, T1, T2, T3, CancellationToken, ValueTask<TNext>> next)
            => self.ThenAsync((v,c) => next(v.Item1, v.Item2, v.Item3, v.Item4, c));

        /// <inheritdoc cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenTask(Func{T, CancellationToken, Task{Outcome{TNext}}})"/>
        public AsyncOutcome<TNext> ThenTask(Func<T, T1, T2, T3, CancellationToken, Task<Outcome<TNext>>> next)
            => self.ThenTask((v, c) => next(v.Item1, v.Item2, v.Item3, v.Item4, c));

        /// <inheritdoc cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenTask(Func{T, CancellationToken, Task{TNext}})"/>
        public AsyncOutcome<TNext> ThenTask(Func<T, T1, T2, T3, CancellationToken, Task<TNext>> next)
            => self.ThenTask((v, c) => next(v.Item1, v.Item2, v.Item3, v.Item4, c));
    }
}
