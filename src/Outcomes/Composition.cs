namespace WarpCode.Outcomes;

/// <summary>
/// Extensions to allow composition via `Then`
/// </summary>
public static class Composition
{
    /// <summary>
    /// Produces an outcome from a map function.
    /// <remarks>
    /// The map function is invoked if the outcome does not contain a problem.
    /// Otherwise the function is never invoked and an outcome returned that carries the problem.
    /// </remarks>
    /// </summary>
    public static Outcome<TNext> Then<T, TNext>(
        this Outcome<T> self,
        Func<T, TNext> map) =>
        self.Match(
            value => new Outcome<TNext>(map(value)),
            problem => new Outcome<TNext>(problem)
        );

    /// <summary>
    /// Produces an outcome from a factory function.
    /// <remarks>
    /// The factory function is invoked if the outcome does not contain a problem.
    /// Otherwise the function is never invoked and an outcome returned that carries the problem.
    /// </remarks>
    /// </summary>
    public static Outcome<TNext> Then<T, TNext>(
        this Outcome<T> self,
        Func<T, Outcome<TNext>> factory) =>
        self.Match(
            factory,
            problem => new Outcome<TNext>(problem)
        );

    /// <summary>
    /// Produces an outcome from a factory function that returns Outcome{None}.
    /// <remarks>
    /// The factory function is invoked if the outcome does not contain a problem.
    /// Otherwise the function is never invoked and an outcome returned that carries the problem.
    /// If the outcome returned by the function holds no problem, the original outcome is returned.
    /// If it holds a problem, an outcome is returned that carries the problem.
    /// </remarks>
    /// </summary>
    public static Outcome<T> Then<T>(
        this Outcome<T> self,
        Func<T, Outcome<None>> factory) =>
        self.Match(
            value => factory(value).Then(_ => self),
            _ => self
        );
}
