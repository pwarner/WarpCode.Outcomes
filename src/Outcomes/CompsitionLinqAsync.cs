namespace Outcomes;

/// <summary>
/// Extensions to allow composition via LINQ natural query with async outcomes
/// </summary>
public static class CompsitionLinqAsync
{
    #region Extensions on Task<Outcome<T>>

    /// <inheritdoc cref="CompositionLinq.Select{T,TNext}(Outcome{T},Func{T,TNext})"/>
    public static async Task<Outcome<TNext>> Select<T, TNext>(
        this Task<Outcome<T>> self,
        Func<T, TNext> map) =>
        (await self.ConfigureAwait(false)).Select(map);

    /// <inheritdoc cref="CompositionLinq.SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
    public static Task<Outcome<TResult>> SelectMany<T, TNext, TResult>(
        this Outcome<T> self,
        Func<T, Task<Outcome<TNext>>> factory,
        Func<T, TNext, TResult> projector) =>
        self.ThenAsync(value => factory(value)
            .ThenAsync(next => projector(value, next))
        );

    /// <inheritdoc cref="CompositionLinq.SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
    public static async Task<Outcome<TResult>> SelectMany<T, TNext, TResult>(
        this Task<Outcome<T>> self,
        Func<T, Outcome<TNext>> factory,
        Func<T, TNext, TResult> projector) =>
        (await self.ConfigureAwait(false)).SelectMany(factory, projector);

    /// <inheritdoc cref="CompositionLinq.SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
    public static async Task<Outcome<TResult>> SelectMany<T, TNext, TResult>(
        this Task<Outcome<T>> self,
        Func<T, Task<Outcome<TNext>>> factory,
        Func<T, TNext, TResult> projector) =>
        await (await self.ConfigureAwait(false))
            .SelectMany(factory, projector)
            .ConfigureAwait(false);

    /// <inheritdoc cref="CompositionLinq.SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
    public static async ValueTask<Outcome<TResult>> SelectMany<T, TNext, TResult>(
        this Task<Outcome<T>> self,
        Func<T, ValueTask<Outcome<TNext>>> factory,
        Func<T, TNext, TResult> projector) =>
        await (await self.ConfigureAwait(false))
            .SelectMany(factory, projector)
            .ConfigureAwait(false);

    #endregion

    #region Extensions on ValueTask<Outcome<T>>

    /// <inheritdoc cref="CompositionLinq.Select{T,TNext}(Outcome{T},Func{T,TNext})"/>
    public static async ValueTask<Outcome<TNext>> Select<T, TNext>(
        this ValueTask<Outcome<T>> self,
        Func<T, TNext> selector) =>
        (await self.ConfigureAwait(false)).Select(selector);

    /// <inheritdoc cref="CompositionLinq.SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
    public static ValueTask<Outcome<TResult>> SelectMany<T, TNext, TResult>(
        this Outcome<T> self,
        Func<T, ValueTask<Outcome<TNext>>> factory,
        Func<T, TNext, TResult> projector) =>
        self.ThenAsync(value => factory(value)
            .ThenAsync(next => projector(value, next))
        );

    /// <inheritdoc cref="CompositionLinq.SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
    public static async ValueTask<Outcome<TResult>> SelectMany<T, TNext, TResult>(
        this ValueTask<Outcome<T>> self,
        Func<T, Outcome<TNext>> selector,
        Func<T, TNext, TResult> projector) =>
        (await self.ConfigureAwait(false)).SelectMany(selector, projector);

    /// <inheritdoc cref="CompositionLinq.SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
    public static async Task<Outcome<TResult>> SelectMany<T, TNext, TResult>(
        this ValueTask<Outcome<T>> self,
        Func<T, Task<Outcome<TNext>>> selector,
        Func<T, TNext, TResult> projector) =>
        await (await self.ConfigureAwait(false))
            .SelectMany(selector, projector)
            .ConfigureAwait(false);

    /// <inheritdoc cref="CompositionLinq.SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
    public static async ValueTask<Outcome<TResult>> SelectMany<T, TNext, TResult>(
        this ValueTask<Outcome<T>> self,
        Func<T, ValueTask<Outcome<TNext>>> selector,
        Func<T, TNext, TResult> projector) =>
        await (await self.ConfigureAwait(false))
            .SelectMany(selector, projector)
            .ConfigureAwait(false);

    #endregion
}
