namespace WarpCode.Outcomes;

/// <summary>
/// Extensions to allow composition via LINQ natural query with async outcomes
/// </summary>
public static class CompsitionLinqAsync
{
    #region Extensions on Task<Outcome<T>>

    extension<T>(Task<Outcome<T>> self)
    {
        /// <inheritdoc cref="CompositionLinq.Select{T,TNext}(Outcome{T},Func{T,TNext})"/>
        public async Task<Outcome<TNext>> Select<TNext>(Func<T, TNext> map) =>
            (await self.ConfigureAwait(false)).Select(map);

        /// <inheritdoc cref="CompositionLinq.SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
        public async Task<Outcome<TResult>> SelectMany<TNext, TResult>(
            Func<T, Outcome<TNext>> factory,
            Func<T, TNext, TResult> projector) =>
            (await self.ConfigureAwait(false)).SelectMany(factory, projector);

        /// <inheritdoc cref="CompositionLinq.SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
        public async Task<Outcome<TResult>> SelectMany<TNext, TResult>(
            Func<T, Task<Outcome<TNext>>> factory,
            Func<T, TNext, TResult> projector) =>
            await (await self.ConfigureAwait(false))
                .SelectMany(factory, projector)
                .ConfigureAwait(false);

        /// <inheritdoc cref="CompositionLinq.SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
        public async ValueTask<Outcome<TResult>> SelectMany<TNext, TResult>(
            Func<T, ValueTask<Outcome<TNext>>> factory,
            Func<T, TNext, TResult> projector) =>
            await (await self.ConfigureAwait(false))
                .SelectMany(factory, projector)
                .ConfigureAwait(false);
    }

    extension<T>(Outcome<T> self)
    {
        /// <inheritdoc cref="CompositionLinq.SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
        public Task<Outcome<TResult>> SelectMany<TNext, TResult>(
            Func<T, Task<Outcome<TNext>>> factory,
            Func<T, TNext, TResult> projector) =>
            self.ThenAsync(value => factory(value)
                .ThenAsync(next => projector(value, next))
            );

        /// <inheritdoc cref="CompositionLinq.SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
        public ValueTask<Outcome<TResult>> SelectMany<TNext, TResult>(
            Func<T, ValueTask<Outcome<TNext>>> factory,
            Func<T, TNext, TResult> projector) =>
            self.ThenAsync(value => factory(value)
                .ThenAsync(next => projector(value, next))
            );
    }

    #endregion

    #region Extensions on ValueTask<Outcome<T>>

    extension<T>(ValueTask<Outcome<T>> self)
    {
        /// <inheritdoc cref="CompositionLinq.Select{T,TNext}(Outcome{T},Func{T,TNext})"/>
        public async ValueTask<Outcome<TNext>> Select<TNext>(Func<T, TNext> selector) =>
            (await self.ConfigureAwait(false)).Select(selector);

        /// <inheritdoc cref="CompositionLinq.SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
        public async ValueTask<Outcome<TResult>> SelectMany<TNext, TResult>(
            Func<T, Outcome<TNext>> selector,
            Func<T, TNext, TResult> projector) =>
            (await self.ConfigureAwait(false)).SelectMany(selector, projector);

        /// <inheritdoc cref="CompositionLinq.SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
        public async Task<Outcome<TResult>> SelectMany<TNext, TResult>(
            Func<T, Task<Outcome<TNext>>> selector,
            Func<T, TNext, TResult> projector) =>
            await (await self.ConfigureAwait(false))
                .SelectMany(selector, projector)
                .ConfigureAwait(false);

        /// <inheritdoc cref="CompositionLinq.SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
        public async ValueTask<Outcome<TResult>> SelectMany<TNext, TResult>(
            Func<T, ValueTask<Outcome<TNext>>> selector,
            Func<T, TNext, TResult> projector) =>
            await (await self.ConfigureAwait(false))
                .SelectMany(selector, projector)
                .ConfigureAwait(false);
    }

    #endregion
}
