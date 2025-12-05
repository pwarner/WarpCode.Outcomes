namespace WarpCode.Outcomes;

/// <summary>
/// Extensions to allow composition via LINQ natural query
/// </summary>
public static class CompositionLinq
{
    extension<T>(Outcome<T> self)
    {
        /// <summary>
        /// Produces an outcome from a map function.
        /// <remarks>
        /// The map function is invoked if the outcome does not contain a problem.
        /// Otherwise the function is never invoked and a new outcome is produced to carry the problem forward.
        /// </remarks>
        /// </summary>
        public Outcome<TNext> Select<TNext>(Func<T, TNext> map) => self.Then(map);

        /// <summary>
        /// Produces an outcome from a factory function.
        /// <remarks>
        /// The factory function is invoked if the outcome does not contain a problem.
        /// Otherwise the function is never invoked and a new outcome is produced to carry the problem forward.
        /// </remarks>
        /// </summary>
        public Outcome<TResult> SelectMany<TNext, TResult>(
            Func<T, Outcome<TNext>> factory,
            Func<T, TNext, TResult> projector) =>
            self.Then(value => factory(value)
                .Then(next => projector(value, next))
            );
    }
}
