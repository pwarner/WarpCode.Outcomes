namespace Outcomes;

public static class Composition
{
    #region Extensions on Outcome<T>

    /// <summary>
    /// Produces an outcome by evaulating the select expression, if the previous outcome did not contain a problem.
    /// Otherwise the expression is never evaulated and a new problem-outcome is produced to carry the problem forward.
    /// </summary>
    public static Outcome<TNext> Map<T, TNext>(
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
    public static Outcome<TNext> Bind<T, TNext>(
        this Outcome<T> self,
        Func<T, Outcome<TNext>> selector) =>
        self.Match(
            selector,
            problem => new Outcome<TNext>(problem)
        );

    /// <inheritdoc cref="Bind{T,TNext}"/>
    public static Task<Outcome<TNext>> BindAsync<T, TNext>(
        this Outcome<T> self,
        Func<T, Task<Outcome<TNext>>> selector) =>
        self.Match(
            selector,
            problem => Task.FromResult(new Outcome<TNext>(problem))
        );

    /// <inheritdoc cref="Bind{T,TNext}"/>
    public static ValueTask<Outcome<TNext>> BindAsync<T, TNext>(
        this Outcome<T> self,
        Func<T, ValueTask<Outcome<TNext>>> selector) =>
        self.Match(
            selector,
            problem => ValueTask.FromResult(new Outcome<TNext>(problem))
        );

    #endregion

    #region Extensions on Task<Outcome<T>>

    /// <inheritdoc cref="Map{T,TNext}"/>
    public static async Task<Outcome<TNext>> MapAsync<T, TNext>(
        this Task<Outcome<T>> self,
        Func<T, TNext> selector) =>
        (await self.ConfigureAwait(false)).Map(selector);

    /// <inheritdoc cref="Bind{T,TNext}"/>
    public static async Task<Outcome<TNext>> BindAsync<T, TNext>(
        this Task<Outcome<T>> self,
        Func<T, Outcome<TNext>> selector) =>
        (await self.ConfigureAwait(false)).Bind(selector);

    /// <inheritdoc cref="Bind{T,TNext}"/>
    public static async Task<Outcome<TNext>> BindAsync<T, TNext>(
        this Task<Outcome<T>> self,
        Func<T, Task<Outcome<TNext>>> selector) =>
        await (await self.ConfigureAwait(false))
            .BindAsync(selector)
            .ConfigureAwait(false);

    /// <inheritdoc cref="Bind{T,TNext}"/>
    public static async Task<Outcome<TNext>> BindAsync<T, TNext>(
        this Task<Outcome<T>> self,
        Func<T, ValueTask<Outcome<TNext>>> selector) =>
        await (await self.ConfigureAwait(false))
            .BindAsync(selector)
            .ConfigureAwait(false);

    #endregion

    #region Extensions on ValueTask<Outcome<T>>

    /// <inheritdoc cref="Map{T,TNext}"/>
    public static async Task<Outcome<TNext>> MapAsync<T, TNext>(
        this ValueTask<Outcome<T>> self,
        Func<T, TNext> selector) =>
        (await self.ConfigureAwait(false)).Map(selector);

    /// <inheritdoc cref="Bind{T,TNext}"/>
    public static async Task<Outcome<TNext>> BindAsync<T, TNext>(
        this ValueTask<Outcome<T>> self,
        Func<T, Outcome<TNext>> selector) =>
        (await self.ConfigureAwait(false)).Bind(selector);

    /// <inheritdoc cref="Bind{T,TNext}"/>
    public static async Task<Outcome<TNext>> BindAsync<T, TNext>(
        this ValueTask<Outcome<T>> self,
        Func<T, Task<Outcome<TNext>>> selector) =>
        await (await self.ConfigureAwait(false))
            .BindAsync(selector)
            .ConfigureAwait(false);

    /// <inheritdoc cref="Bind{T,TNext}"/>
    public static async Task<Outcome<TNext>> BindAsync<T, TNext>(
        this ValueTask<Outcome<T>> self,
        Func<T, ValueTask<Outcome<TNext>>> selector) =>
        await (await self.ConfigureAwait(false))
            .BindAsync(selector)
            .ConfigureAwait(false);

    #endregion
}
