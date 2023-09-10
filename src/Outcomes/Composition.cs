namespace Outcomes;

public static class Composition
{
    #region Extensions on Outcome<T>

    /// <summary>
    /// Produces an outcome by evaulating the select expression, if the previous outcome did not contain a problem.
    /// Otherwise the expression is never evaulated and a new problem-outcome is produced to carry the problem forward.
    /// </summary>
    public static Outcome<TNext> Select<T, TNext>(
        this Outcome<T> self,
        Func<T, TNext> selector) =>
        self.Match(
            value => Outcome.Ok(selector(value)),
            problem => new Outcome<TNext>(problem)
        );

    /// <summary>
    /// Produces an outcome by evaluating the expression to the right of the "in" keyword, if the previous outcome did not contain a problem.
    /// Otherwise the expression is never evaluated and a new problem-outcome is produced to carry the problem forward.
    /// </summary>
    public static Outcome<TResult> SelectMany<T, TNext, TResult>(
        this Outcome<T> self,
        Func<T, Outcome<TNext>> selector,
        Func<T, TNext, TResult> projector)
    {
        return self.Match(
            value => from next in selector(value) select projector(value, next),
            problem => new Outcome<TResult>(problem)
        );
    }

    /// <inheritdoc cref="SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
    public static Task<Outcome<TResult>> SelectMany<T, TNext, TResult>(
        this Outcome<T> self,
        Func<T, Task<Outcome<TNext>>> selector,
        Func<T, TNext, TResult> projector) =>
        self.Match(
            async value =>
                from next in await selector(value).ConfigureAwait(false)
                select projector(value, next),
            problem => Task.FromResult(new Outcome<TResult>(problem))
        );

    /// <inheritdoc cref="SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
    public static Task<Outcome<TResult>> SelectMany<T, TNext, TResult>(
        this Outcome<T> self,
        Func<T, ValueTask<Outcome<TNext>>> selector,
        Func<T, TNext, TResult> projector) =>
        self.Match(
            async value =>
                from next in await selector(value).ConfigureAwait(false)
                select projector(value, next),
            problem => Task.FromResult(new Outcome<TResult>(problem))
        );

    #endregion

    #region Extensions on Task<Outcome<T>>

    /// <inheritdoc cref="Select{T,TNext}(Outcome{T},Func{T,TNext})"/>
    public static async Task<Outcome<TNext>> Select<T, TNext>(
        this Task<Outcome<T>> self,
        Func<T, TNext> selector) =>
        (await self.ConfigureAwait(false)).Select(selector);

    /// <inheritdoc cref="SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
    public static async Task<Outcome<TResult>> SelectMany<T, TNext, TResult>(
        this Task<Outcome<T>> self,
        Func<T, Outcome<TNext>> selector,
        Func<T, TNext, TResult> projector) =>
        (await self.ConfigureAwait(false)).SelectMany(selector, projector);

    /// <inheritdoc cref="SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
    public static async Task<Outcome<TResult>> SelectMany<T, TNext, TResult>(
        this Task<Outcome<T>> self,
        Func<T, Task<Outcome<TNext>>> selector,
        Func<T, TNext, TResult> projector) =>
        await (await self.ConfigureAwait(false))
            .SelectMany(selector, projector)
            .ConfigureAwait(false);

    /// <inheritdoc cref="SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
    public static async Task<Outcome<TResult>> SelectMany<T, TNext, TResult>(
        this Task<Outcome<T>> self,
        Func<T, ValueTask<Outcome<TNext>>> selector,
        Func<T, TNext, TResult> projector) =>
        await (await self.ConfigureAwait(false))
            .SelectMany(selector, projector)
            .ConfigureAwait(false);

    #endregion

    #region Extensions on ValueTask<Outcome<T>>

    /// <inheritdoc cref="Select{T,TNext}(Outcome{T},Func{T,TNext})"/>
    public static async Task<Outcome<TNext>> Select<T, TNext>(
        this ValueTask<Outcome<T>> self,
        Func<T, TNext> selector) =>
        (await self.ConfigureAwait(false)).Select(selector);

    /// <inheritdoc cref="SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
    public static async Task<Outcome<TResult>> SelectMany<T, TNext, TResult>(
        this ValueTask<Outcome<T>> self,
        Func<T, Outcome<TNext>> selector,
        Func<T, TNext, TResult> projector) =>
        (await self.ConfigureAwait(false)).SelectMany(selector, projector);

    /// <inheritdoc cref="SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
    public static async Task<Outcome<TResult>> SelectMany<T, TNext, TResult>(
        this ValueTask<Outcome<T>> self,
        Func<T, Task<Outcome<TNext>>> selector,
        Func<T, TNext, TResult> projector) =>
        await (await self.ConfigureAwait(false))
            .SelectMany(selector, projector)
            .ConfigureAwait(false);

    /// <inheritdoc cref="SelectMany{T,TNext,TResult}(Outcome{T},Func{T,Outcome{TNext}},Func{T,TNext,TResult})"/>
    public static async Task<Outcome<TResult>> SelectMany<T, TNext, TResult>(
        this ValueTask<Outcome<T>> self,
        Func<T, ValueTask<Outcome<TNext>>> selector,
        Func<T, TNext, TResult> projector) =>
        await (await self.ConfigureAwait(false))
            .SelectMany(selector, projector)
            .ConfigureAwait(false);

    #endregion
}
