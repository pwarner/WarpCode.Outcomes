namespace Outcomes;

/// <summary>
/// Copy of OutcomeExtensions but extending Task{Outcome{T}} and ValueTask{Outcome{T}}.
/// </summary>
public static class OutcomeExtensionsAsync
{
    #region Extensions on Task<Outcome<T>>

    /// <inheritdoc cref="Outcome{T}.Match{TFinal}"/>
    public static async Task<TFinal> MatchAsync<T, TFinal>(
        this Task<Outcome<T>> self,
        Func<T, TFinal> onSuccess,
        Func<IProblem, TFinal> onProblem) =>
        (await self.ConfigureAwait(false)).Match(onSuccess, onProblem);

    /// <inheritdoc cref="OutcomeExtensions.OnSuccess{T}"/>
    public static async Task<Outcome<T>> OnSuccessAsync<T>(
        this Task<Outcome<T>> self,
        Action<T> action) =>
        (await self.ConfigureAwait(false)).OnSuccess(action);

    /// <inheritdoc cref="OutcomeExtensions.OnProblem{T}"/>
    public static async Task<Outcome<T>> OnProblemAsync<T>(
        this Task<Outcome<T>> self,
        Action<IProblem> action) =>
        (await self.ConfigureAwait(false)).OnProblem(action);

    /// <inheritdoc cref="OutcomeExtensions.Ensure{T}"/>
    public static async Task<Outcome<T>> EnsureAsync<T>(
        this Task<Outcome<T>> self,
        Predicate<T> predicate,
        Func<T, IProblem> factory) =>
        (await self.ConfigureAwait(false)).Ensure(predicate, factory);

    /// <inheritdoc cref="OutcomeExtensions.Rescue{T}"/>
    public static async Task<Outcome<T>> RescueAsync<T>(
        this Task<Outcome<T>> self,
        Predicate<IProblem> predicate,
        Func<IProblem, T> factory) =>
        (await self.ConfigureAwait(false)).Rescue(predicate, factory);

    #endregion

    #region Extensions on ValueTask<Outcome<T>>

    /// <inheritdoc cref="Outcome{T}.Match{TFinal}"/>
    public static async ValueTask<TFinal> MatchAsync<T, TFinal>(
        this ValueTask<Outcome<T>> self,
        Func<T, TFinal> onSuccess,
        Func<IProblem, TFinal> onProblem) =>
        (await self.ConfigureAwait(false)).Match(onSuccess, onProblem);

    /// <inheritdoc cref="OutcomeExtensions.OnSuccess{T}"/>
    public static async ValueTask<Outcome<T>> OnSuccessAsync<T>(
        this ValueTask<Outcome<T>> self,
        Action<T> action) =>
        (await self.ConfigureAwait(false)).OnSuccess(action);

    /// <inheritdoc cref="OutcomeExtensions.OnProblem{T}"/>
    public static async ValueTask<Outcome<T>> OnProblemAsync<T>(
        this ValueTask<Outcome<T>> self,
        Action<IProblem> action) =>
        (await self.ConfigureAwait(false)).OnProblem(action);

    /// <inheritdoc cref="OutcomeExtensions.Ensure{T}"/>
    public static async ValueTask<Outcome<T>> EnsureAsync<T>(
        this ValueTask<Outcome<T>> self,
        Predicate<T> predicate,
        Func<T, IProblem> factory) =>
        (await self.ConfigureAwait(false)).Ensure(predicate, factory);

    /// <inheritdoc cref="OutcomeExtensions.Rescue{T}"/>
    public static async ValueTask<Outcome<T>> RescueAsync<T>(
        this ValueTask<Outcome<T>> self,
        Predicate<IProblem> predicate,
        Func<IProblem, T> factory) =>
        (await self.ConfigureAwait(false)).Rescue(predicate, factory);

    #endregion

    #region extensions on IAsyncEnumerable<Outcome<T>>

    /// <inheritdoc cref="OutcomeExtensions.Aggregate{T}"/>
    public static async Task<Outcome<List<T>>> AggregateAsync<T>(
        this IAsyncEnumerable<Outcome<T>> outcomes,
        bool bailEarly = false)
    {
        List<T>? list = null;
        List<IProblem>? problems = null;

        await foreach (Outcome<T> outcome in outcomes.ConfigureAwait(false))
        {
            outcome
                .OnSuccess(x => (list ??= []).Add(x))
                .OnProblem(x => (problems ??= []).Add(x));

            if (problems is not null && bailEarly)
                return problems[0].ToOutcome<List<T>>();
        }

        return problems switch
        {
            not null => new ProblemAggregate(problems),
            null => list!
        };
    }

    /// <inheritdoc cref="OutcomeExtensions.Aggregate"/>
    public static async Task<Outcome<None>> AggregateAsync(
        this IAsyncEnumerable<Outcome<None>> outcomes,
        bool bailEarly = false)
    {
        List<IProblem>? problems = null;

        await foreach (Outcome<None> outcome in outcomes.ConfigureAwait(false))
        {
            outcome.OnProblem(x => (problems ??= []).Add(x));

            if (problems is not null && bailEarly)
                return problems[0].ToOutcome();
        }

        return problems switch
        {
            not null => new ProblemAggregate(problems),
            null => Outcome.Ok
        };
    }

    #endregion
}
