namespace WarpCode.Outcomes;

/// <summary>
/// `Then` composition extensions for async outcomes
/// </summary>
public static class AsyncThenExtensions
{
    /// <param name="self">Current async outcome.</param>
    extension<T, TNext>(AsyncOutcome<T> self)
    {
        /// <summary>
        /// Wraps an async call to the `Then` extension on the underlying outcome.
        /// </summary>
        private async ValueTask<Outcome<TNext>> Local(Func<T, Outcome<TNext>> next) 
            => (await self).Then(next);

        /// <summary>
        /// Async happy-path composition operator.
        /// If the async outcome resolves to a valid value, the `next` function is executed to produce the next outcome.
        /// If the async outcome resolves to a problem, the `next` function is not executed and the problem is propagated.
        /// </summary>
        /// <param name="next">Function to execute to obtain next outcome.</param>
        /// <returns>An AsyncOutcome{TNext} representing the result of the composition.</returns>
        public AsyncOutcome<TNext> Then(Func<T, Outcome<TNext>> next) 
            => self.With(self.Local(next));

        /// <summary>
        /// Async happy-path composition operator.
        /// If the async outcome resolves to a valid value, the `next` function is executed to produce the next outcome.
        /// If the async outcome resolves to a problem, the `next` function is not executed and the problem is propagated.
        /// </summary>
        /// <param name="next">Function to execute to obtain next value.</param>
        /// <returns>An AsyncOutcome{TNext} representing the result of the composition.</returns>
        public AsyncOutcome<TNext> Then(Func<T, TNext> next) 
            => self.Then(value => new Outcome<TNext>(next(value)));
    }

    /// <summary>Then extensions for an outcome containing no value.</summary>
    /// <param name="self">Current value-less outcome.</param>
    extension<TNext>(AsyncOutcome<None> self)
    {
        /// <inheritdoc cref=" AsyncThenExtensions.extension{T,TNext}(AsyncOutcome{T}).Then(Func{T, Outcome{TNext}})"/>
        public AsyncOutcome<TNext> Then(Func<Outcome<TNext>> next)
            => self.Then(_ => next());

        /// <inheritdoc cref=" AsyncThenExtensions.extension{T,TNext}(AsyncOutcome{T}).Then(Func{T, TNext})"/>
        public AsyncOutcome<TNext> Then(Func<TNext> next)
            => self.Then(_ => new Outcome<TNext>(next()));

        /// <summary>
        /// Async happy-path composition operator.
        /// If the async outcome resolves to no problem, the `next` outcome is returned.
        /// If the async outcome resolves to a problem, the `next` outcome is not returned and the problem is propagated.
        /// </summary>
        /// <param name="next">Outcome to return if the current outcome is successful.</param>
        /// <returns>An AsyncOutcome{TNext} representing the result of the composition.</returns>
        public AsyncOutcome<TNext> Then(Outcome<TNext> next)
            => self.Then(_ => next);

        /// <summary>
        /// Async happy-path composition operator.
        /// If the async outcome resolves to no problem, the `next` value is returned as an outcome.
        /// If the async outcome resolves to a problem, the `next` outcome is not returned and the problem is propagated.
        /// </summary>
        /// <param name="next">Value to return as an outcome if the current outcome is successful.</param>
        /// <returns>An AsyncOutcome{TNext} representing the result of the composition.</returns>
        public AsyncOutcome<TNext> Then(TNext next)
            => self.Then(_ => new Outcome<TNext>(next));
    }

    /// <summary>Then extensions for an async outcome containing a tuple of two values.</summary>
    /// <param name="self">Current async outcome.</param>
    extension<T, T1, TNext>(AsyncOutcome<(T, T1)> self)
    {
        /// <inheritdoc cref="AsyncThenExtensions.extension{T,TNext}(AsyncOutcome{T}).Then(Func{T, Outcome{TNext}})"/>
        public AsyncOutcome<TNext> Then(Func<T, T1, Outcome<TNext>> next)
            => self.Then(value => next(value.Item1, value.Item2));

        /// <inheritdoc cref="AsyncThenExtensions.extension{T,TNext}(AsyncOutcome{T}).Then(Func{T, TNext})"/>
        public AsyncOutcome<TNext> Then(Func<T, T1, TNext> next)
            => self.Then(value => next(value.Item1, value.Item2));
    }

    /// <summary>Then extensions for an async outcome containing a tuple of three values.</summary>
    /// <param name="self">Current async outcome.</param>
    extension<T, T1, T2, TNext>(AsyncOutcome<(T, T1, T2)> self)
    {
        /// <inheritdoc cref="AsyncThenExtensions.extension{T,TNext}(AsyncOutcome{T}).Then(Func{T, Outcome{TNext}})"/>
        public AsyncOutcome<TNext> Then(Func<T, T1, T2, Outcome<TNext>> next)
            => self.Then(value => next(value.Item1, value.Item2, value.Item3));

        /// <inheritdoc cref="AsyncThenExtensions.extension{T,TNext}(AsyncOutcome{T}).Then(Func{T, TNext})"/>
        public AsyncOutcome<TNext> Then(Func<T, T1, T2, TNext> next)
            => self.Then(value => next(value.Item1, value.Item2, value.Item3));
    }

    /// <summary>Then extensions for an async outcome containing a tuple of four values.</summary>
    /// <param name="self">Current async outcome.</param>
    extension<T, T1, T2, T3, TNext>(AsyncOutcome<(T, T1, T2, T3)> self)
    {
        /// <inheritdoc cref="AsyncThenExtensions.extension{T,TNext}(AsyncOutcome{T}).Then(Func{T, Outcome{TNext}})"/>
        public AsyncOutcome<TNext> Then(Func<T, T1, T2, T3, Outcome<TNext>> next)
            => self.Then(value => next(value.Item1, value.Item2, value.Item3, value.Item4));

        /// <inheritdoc cref="AsyncThenExtensions.extension{T,TNext}(AsyncOutcome{T}).Then(Func{T, TNext})"/>
        public AsyncOutcome<TNext> Then(Func<T, T1, T2, T3, TNext> next)
            => self.Then(value => next(value.Item1, value.Item2, value.Item3, value.Item4));
    }
}