namespace WarpCode.Outcomes;

/// <summary>
/// Copy of OutcomeExtensions but extending Task{Outcome{T}} and ValueTask{Outcome{T}}.
/// </summary>
public static class OutcomeExtensionsAsync
{
    #region Extensions on Task<Outcome<T>>

    extension<T>(Task<Outcome<T>> self)
    {
        /// <inheritdoc cref="Outcome{T}.Match{TFinal}"/>
        public async Task<TFinal> MatchAsync<TFinal>(
            Func<T, TFinal> onSuccess,
            Func<IProblem, TFinal> onProblem) =>
            (await self.ConfigureAwait(false)).Match(onSuccess, onProblem);

        /// <inheritdoc cref="OutcomeExtensions.OnSuccess{T}"/>
        public async Task<Outcome<T>> OnSuccessAsync(Action<T> action) =>
            (await self.ConfigureAwait(false)).OnSuccess(action);

        /// <inheritdoc cref="OutcomeExtensions.OnProblem{T}"/>
        public async Task<Outcome<T>> OnProblemAsync(Action<IProblem> action) =>
            (await self.ConfigureAwait(false)).OnProblem(action);

        /// <inheritdoc cref="OutcomeExtensions.Ensure{T}"/>
        public async Task<Outcome<T>> EnsureAsync(
            Predicate<T> predicate,
            Func<T, IProblem> factory) =>
            (await self.ConfigureAwait(false)).Ensure(predicate, factory);

        /// <inheritdoc cref="OutcomeExtensions.Rescue{T}"/>
        public async Task<Outcome<T>> RescueAsync(
            Predicate<IProblem> predicate,
            Func<IProblem, T> factory) =>
            (await self.ConfigureAwait(false)).Rescue(predicate, factory);
    }

    #endregion

    #region Extensions on ValueTask<Outcome<T>>

    extension<T>(ValueTask<Outcome<T>> self)
    {
        /// <inheritdoc cref="Outcome{T}.Match{TFinal}"/>
        public async ValueTask<TFinal> MatchAsync<TFinal>(
            Func<T, TFinal> onSuccess,
            Func<IProblem, TFinal> onProblem) =>
            (await self.ConfigureAwait(false)).Match(onSuccess, onProblem);

        /// <inheritdoc cref="OutcomeExtensions.OnSuccess{T}"/>
        public async ValueTask<Outcome<T>> OnSuccessAsync(Action<T> action) =>
            (await self.ConfigureAwait(false)).OnSuccess(action);

        /// <inheritdoc cref="OutcomeExtensions.OnProblem{T}"/>
        public async ValueTask<Outcome<T>> OnProblemAsync(Action<IProblem> action) =>
            (await self.ConfigureAwait(false)).OnProblem(action);

        /// <inheritdoc cref="OutcomeExtensions.Ensure{T}"/>
        public async ValueTask<Outcome<T>> EnsureAsync(
            Predicate<T> predicate,
            Func<T, IProblem> factory) =>
            (await self.ConfigureAwait(false)).Ensure(predicate, factory);

        /// <inheritdoc cref="OutcomeExtensions.Rescue{T}"/>
        public async ValueTask<Outcome<T>> RescueAsync(
            Predicate<IProblem> predicate,
            Func<IProblem, T> factory) =>
            (await self.ConfigureAwait(false)).Rescue(predicate, factory);
    }

    #endregion

    #region Extensions on IAsyncEnumerable<Outcome<T>>

    extension<T>(IAsyncEnumerable<Outcome<T>> outcomes)
    {
        /// <inheritdoc cref="OutcomeExtensions.Aggregate{T}"/>
        public async Task<Outcome<List<T>>> AggregateAsync(bool bailEarly = false)
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
    }

    extension(IAsyncEnumerable<Outcome<None>> outcomes)
    {
        /// <inheritdoc cref="OutcomeExtensions.Aggregate"/>
        public async Task<Outcome<None>> AggregateAsync(bool bailEarly = false)
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
    }

    #endregion
}
