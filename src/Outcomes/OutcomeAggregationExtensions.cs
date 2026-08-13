namespace WarpCode.Outcomes;

/// <summary>
/// Extensions for aggregating multiple <see cref="Outcome{T}"/> into a single <see cref="Outcome{T}"/> containing 
/// a collection of the successful values, or a collection of the problems encountered.
/// </summary>
public static class OutcomeAggregationExtensions
{
    extension<T>(IEnumerable<Outcome<T>> outcomes)
    {
        /// <summary>
        /// Aggregates a set of Outcome{T} to produce a single Outcome.
        /// </summary>
        /// <remarks>
        /// The outcome will contain either:
        /// A <see cref="List{T}"/> value if none of the input outcomes held a problem.
        /// An outcome of the first problem in the sequence if <paramref name="bailEarly"/> was true.
        /// An outcome of a <see cref="ProblemAggregate"/> containing each of the problems found.
        /// </remarks>
        /// <param name="bailEarly">Whether to return the first problem discovered, or collect all problems.</param>
        /// <returns>An <see cref="Outcome{T}"/> of type T[].</returns>
        public Outcome<T[]> Aggregate(bool bailEarly = false)
        {
            List<T>? values = null;
            List<Problem>? problems = null;
            int? capacity = outcomes.TryGetNonEnumeratedCount(out var count) ? count : null;

            foreach (Outcome<T> outcome in outcomes)
            {
                if (outcome.Problem is not null)
                    Collect(ref problems, capacity, outcome.Problem);
                else
                    Collect(ref values, capacity, outcome.Value);

                if (problems is not null && bailEarly)
                    return problems[0];
            }

            return (values, problems) switch
            {
                (_, not null) => new ProblemAggregate([.. problems]),
                ({ } items, _) => new([.. items]),
                _ => Array.Empty<T>()
            };
        }
    }

    extension(IEnumerable<Outcome<None>> outcomes)
    {
        /// <summary>
        /// Aggregates a set of Outcome{None} to produce a single Outcome.
        /// </summary>
        /// <remarks>
        /// The outcome will contain either:
        /// A successful Outcome{None} if none of the input outcomes held a problem.
        /// An outcome of the first problem in the sequence if <paramref name="bailEarly"/> was true.
        /// An outcome of a <see cref="ProblemAggregate"/> containing each of the problems found.
        /// </remarks>
        /// <param name="bailEarly">Whether to return the first problem discovered, or collect all problems.</param>
        /// <returns>An <see cref="Outcome{None}"/>.</returns>
        public Outcome<None> Aggregate(bool bailEarly = false)
        {
            List<Problem>? problems = null;
            int? capacity = outcomes.TryGetNonEnumeratedCount(out var count) ? count : null;

            foreach (Outcome<None> outcome in outcomes)
            {
                if (outcome.Problem is not null)
                    Collect(ref problems, capacity, outcome.Problem);

                if (problems is not null && bailEarly)
                    return new(problems[0]);
            }

            return problems switch
            {
                not null => new ProblemAggregate([.. problems]),
                null => Outcome.Ok
            };
        }
    }

    extension<T>(IAsyncEnumerable<Outcome<T>> outcomes)
    {
        /// <inheritdoc cref="OutcomeAggregationExtensions.Aggregate{T}"/>
        public async Task<Outcome<T[]>> AggregateAsync(bool bailEarly = false)
        {
            List<T>? values = null;
            List<Problem>? problems = null;

            await foreach (Outcome<T> outcome in outcomes.ConfigureAwait(false))
            {
                if (outcome.Problem is not null)
                    Collect(ref problems, null, outcome.Problem);
                else
                    Collect(ref values, null, outcome.Value);

                if (problems is not null && bailEarly)
                    return problems[0];
            }

            return (values, problems) switch
            {
                (_, not null) => new ProblemAggregate([.. problems]),
                ({ } items, _) => new([.. items]),
                _ => Array.Empty<T>()
            };
        }
    }

    extension(IAsyncEnumerable<Outcome<None>> outcomes)
    {
        /// <inheritdoc cref="OutcomeAggregationExtensions.Aggregate"/>
        public async Task<Outcome<None>> AggregateAsync(bool bailEarly = false)
        {
            List<Problem>? problems = null;

            await foreach (Outcome<None> outcome in outcomes.ConfigureAwait(false))
            {
                if (outcome.Problem is not null)
                    Collect(ref problems, null, outcome.Problem);

                if (problems is not null && bailEarly)
                    return new(problems[0]);
            }

            return problems switch
            {
                not null => new ProblemAggregate([.. problems]),
                null => Outcome.Ok
            };
        }
    }

    private static void Collect<T>(ref List<T>? list, int? capacity, T item)
    {
        list ??= capacity.HasValue ? new List<T>(capacity.Value) : [];
        list.Add(item);
    }
}