namespace WarpCode.Outcomes;

/// <summary>
/// `Then` composition extensions for async outcomes
/// </summary>
public static class ThenAsyncExtensions
{
    /// <param name="self">Current async outcome.</param>
    extension<T, TNext>(AsyncOutcome<T> self)
    {
        /// <summary>
        /// Wraps an async call to the `Then` extension on the underlying outcome.
        /// </summary>
        private async ValueTask<Outcome<TNext>> Local(Func<T, CancellationToken, ValueTask<Outcome<TNext>>> next)
            => await (await self).Match(
                value => next(value, self.CancellationToken),
                static problem => ValueTask.FromResult(new Outcome<TNext>(problem))
            );

        /// <summary>
        /// Async happy-path composition operator.
        /// If the async outcome resolves to a valid value, the `next` function is executed to produce the next outcome.
        /// If the async outcome resolves to a problem, the `next` function is not executed and the problem is propagated.
        /// </summary>
        /// <param name="next">Function to execute to obtain next outcome.</param>
        /// <returns>An AsyncOutcome{TNext} representing the result of the composition.</returns>
        public AsyncOutcome<TNext> ThenAsync(Func<T, CancellationToken, ValueTask<Outcome<TNext>>> next) 
            => self.With(self.Local(next));

        /// <inheritdoc cref=" ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{Outcome{TNext}}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<T, CancellationToken, Task<Outcome<TNext>>> next)
            => self.ThenAsync((v,c)=> new ValueTask<Outcome<TNext>>(next(v,c)));

        /// <summary>
        /// Async happy-path composition operator.
        /// If the async outcome resolves to a valid value, the `next` function is executed to produce the next outcome.
        /// If the async outcome resolves to a problem, the `next` function is not executed and the problem is propagated.
        /// </summary>
        /// <param name="next">Function to execute to obtain next value.</param>
        /// <returns>An AsyncOutcome{TNext} representing the result of the composition.</returns>
        public AsyncOutcome<TNext> ThenAsync(Func<T, CancellationToken, ValueTask<TNext>> next) 
            => self.With(self.Local<T, TNext>(async (v, c)=> new(await next(v, c))));

        /// <inheritdoc cref=" ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{TNext}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<T, CancellationToken, Task<TNext>> next)
            => self.ThenAsync((v, c) => new ValueTask<TNext>(next(v, c)));
    }

    /// <summary>Then extensions for an outcome containing no value.</summary>
    /// <param name="self">Current value-less outcome.</param>
    extension<TNext>(AsyncOutcome<None> self)
    {
        /// <inheritdoc cref=" ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{Outcome{TNext}}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<CancellationToken, ValueTask<Outcome<TNext>>> next)
            => self.ThenAsync((_, c) => next(c));

        /// <inheritdoc cref=" ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{Outcome{TNext}}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<CancellationToken, Task<Outcome<TNext>>> next)
            => self.ThenAsync((_,c)=> new ValueTask<Outcome<TNext>>(next(c)));

        /// <inheritdoc cref=" ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{TNext}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<CancellationToken, ValueTask<TNext>> next)
            => self.ThenAsync((_, c) => next(c));

        /// <inheritdoc cref=" ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{TNext}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<CancellationToken, Task<TNext>> next)
            => self.ThenAsync((_, c) => new ValueTask<TNext>(next(c)));
    }

    /// <summary>Then extensions for an async outcome containing a tuple of two values.</summary>
    /// <param name="self">Current async outcome.</param>
    extension<T, T1, TNext>(AsyncOutcome<(T, T1)> self)
    {
        /// <inheritdoc cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{Outcome{TNext}}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<T, T1, CancellationToken, ValueTask<Outcome<TNext>>> next)
            => self.ThenAsync((v, c) => next(v.Item1, v.Item2, c));

        /// <inheritdoc cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{Outcome{TNext}}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<T, T1, CancellationToken, Task<Outcome<TNext>>> next)
            => self.ThenAsync((v, c) => next(v.Item1, v.Item2, c));

        /// <inheritdoc cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{TNext}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<T, T1, CancellationToken, ValueTask<TNext>> next)
            => self.ThenAsync((v, c) => next(v.Item1, v.Item2, c));

        /// <inheritdoc cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{TNext}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<T, T1, CancellationToken, Task<TNext>> next)
            => self.ThenAsync((v, c) => next(v.Item1, v.Item2, c));
    }

    /// <summary>Then extensions for an async outcome containing a tuple of three values.</summary>
    /// <param name="self">Current async outcome.</param>
    extension<T, T1, T2, TNext>(AsyncOutcome<(T, T1, T2)> self)
    {
        /// <inheritdoc cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{Outcome{TNext}}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<T, T1, T2, CancellationToken, ValueTask<Outcome<TNext>>> next)
            => self.ThenAsync((v,c) => next(v.Item1, v.Item2, v.Item3, c));
        
        /// <inheritdoc cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{Outcome{TNext}}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<T, T1, T2, CancellationToken, Task<Outcome<TNext>>> next)
            => self.ThenAsync((v, c) => next(v.Item1, v.Item2, v.Item3, c));

        /// <inheritdoc cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{TNext}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<T, T1, T2, CancellationToken, ValueTask<TNext>> next)
            => self.ThenAsync((v,c) => next(v.Item1, v.Item2, v.Item3, c));

        /// <inheritdoc cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{TNext}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<T, T1, T2, CancellationToken, Task<TNext>> next)
            => self.ThenAsync((v, c) => next(v.Item1, v.Item2, v.Item3, c));
    }

    /// <summary>Then extensions for an async outcome containing a tuple of four values.</summary>
    /// <param name="self">Current async outcome.</param>
    extension<T, T1, T2, T3, TNext>(AsyncOutcome<(T, T1, T2, T3)> self)
    {
        /// <inheritdoc cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{Outcome{TNext}}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<T, T1, T2, T3, CancellationToken, ValueTask<Outcome<TNext>>> next)
            => self.ThenAsync((v,c) => next(v.Item1, v.Item2, v.Item3, v.Item4, c));

        /// <inheritdoc cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{Outcome{TNext}}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<T, T1, T2, T3, CancellationToken, Task<Outcome<TNext>>> next)
            => self.ThenAsync((v, c) => next(v.Item1, v.Item2, v.Item3, v.Item4, c));

        /// <inheritdoc cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{TNext}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<T, T1, T2, T3, CancellationToken, ValueTask<TNext>> next)
            => self.ThenAsync((v,c) => next(v.Item1, v.Item2, v.Item3, v.Item4, c));

        /// <inheritdoc cref="ThenAsyncExtensions.extension{T,TNext}(AsyncOutcome{T}).ThenAsync(Func{T, CancellationToken, ValueTask{TNext}})"/>
        public AsyncOutcome<TNext> ThenAsync(Func<T, T1, T2, T3, CancellationToken, Task<TNext>> next)
            => self.ThenAsync((v, c) => next(v.Item1, v.Item2, v.Item3, v.Item4, c));
    }
}