namespace WarpCode.Outcomes;

/// <summary>
/// Extensions for aggregating multiple <see cref="Result{T}"/> into a single <see cref="Result{T}"/> containing 
/// a collection of the successful values, or a collection of the problems encountered.
/// </summary>
public static class ResultAggregationExtensions
{
    extension<T>(IEnumerable<Result<T>> results)
    {
        /// <summary>
        /// Aggregates a set of Result{T} to produce a single Result.
        /// </summary>
        /// <remarks>
        /// The result will contain either:
        /// A <see cref="List{T}"/> value if none of the input results held a problem.
        /// A result of the first problem in the sequence if <paramref name="bailEarly"/> was true.
        /// A result of a <see cref="ProblemAggregate"/> containing each of the problems found.
        /// </remarks>
        /// <param name="bailEarly">Whether to return the first problem discovered, or collect all problems.</param>
        /// <returns>An <see cref="Result{T}"/> of type T[].</returns>
        public Result<T[]> Aggregate(bool bailEarly = false)
        {
            List<T>? values = null;
            List<Problem>? problems = null;
            int? capacity = results.TryGetNonEnumeratedCount(out var count) ? count : null;

            foreach (Result<T> result in results)
            {
                if (result.Problem is not null)
                    Collect(ref problems, capacity, result.Problem);
                else
                    Collect(ref values, capacity, result.Value);

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

    extension(IEnumerable<Result<None>> results)
    {
        /// <summary>
        /// Aggregates a set of Result{None} to produce a single Result.
        /// </summary>
        /// <remarks>
        /// The result will contain either:
        /// A successful Result{None} if none of the input results held a problem.
        /// A result of the first problem in the sequence if <paramref name="bailEarly"/> was true.
        /// A result of a <see cref="ProblemAggregate"/> containing each of the problems found.
        /// </remarks>
        /// <param name="bailEarly">Whether to return the first problem discovered, or collect all problems.</param>
        /// <returns>An <see cref="Result{None}"/>.</returns>
        public Result<None> Aggregate(bool bailEarly = false)
        {
            List<Problem>? problems = null;
            int? capacity = results.TryGetNonEnumeratedCount(out var count) ? count : null;

            foreach (Result<None> result in results)
            {
                if (result.Problem is not null)
                    Collect(ref problems, capacity, result.Problem);

                if (problems is not null && bailEarly)
                    return new(problems[0]);
            }

            return problems switch
            {
                not null => new ProblemAggregate([.. problems]),
                null => Result.Ok
            };
        }
    }

    extension<T>(IAsyncEnumerable<Result<T>> results)
    {
        /// <inheritdoc cref="ResultAggregationExtensions.Aggregate{T}"/>
        public async Task<Result<T[]>> AggregateAsync(bool bailEarly = false)
        {
            List<T>? values = null;
            List<Problem>? problems = null;

            await foreach (Result<T> result in results.ConfigureAwait(false))
            {
                if (result.Problem is not null)
                    Collect(ref problems, null, result.Problem);
                else
                    Collect(ref values, null, result.Value);

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

    extension(IAsyncEnumerable<Result<None>> results)
    {
        /// <inheritdoc cref="ResultAggregationExtensions.Aggregate"/>
        public async Task<Result<None>> AggregateAsync(bool bailEarly = false)
        {
            List<Problem>? problems = null;

            await foreach (Result<None> result in results.ConfigureAwait(false))
            {
                if (result.Problem is not null)
                    Collect(ref problems, null, result.Problem);

                if (problems is not null && bailEarly)
                    return new(problems[0]);
            }

            return problems switch
            {
                not null => new ProblemAggregate([.. problems]),
                null => Result.Ok
            };
        }
    }

    private static void Collect<T>(ref List<T>? list, int? capacity, T item)
    {
        list ??= capacity.HasValue ? new List<T>(capacity.Value) : [];
        list.Add(item);
    }
}