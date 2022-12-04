namespace Outcomes;

public static class ProblemResolverExtensions
{
    /// <summary>
    /// Operation to produce a final value from this outcome.
    /// </summary>
    /// <typeparam name="TSource">The type of the source outcome value.</typeparam>
    /// <typeparam name="TResult">The type of the result outcome value.</typeparam>
    /// <typeparam name="TP1">The type of the first strong-typed problem resolver.</typeparam>
    /// <param name="self">The source outcome on which to operate.</param>
    /// <param name="onSuccess">Transformation function to apply when there is no problem.</param>
    /// <param name="fallback">Transformation function to apply when there is a problem not matched by a strong-typed resolver.</param>
    /// <param name="onProblem1">Problem resolver for problems of type <typeparam name="TP1"/>.</param>
    /// <returns>The result of applying one of the transformation functions.</returns>
    /// <exception cref="ArgumentNullException">Thrown when either onSuccess or any problem resolver is null.</exception>
    public static TResult Resolve<TSource, TP1, TResult>(
        this Outcome<TSource> self,
        Func<TSource, TResult> onSuccess,
        Func<IProblem, TResult> fallback,
        Func<TP1, TResult> onProblem1)
    {
        if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
        if (fallback == null) throw new ArgumentNullException(nameof(fallback));
        if (onProblem1 == null) throw new ArgumentNullException(nameof(onProblem1));

        return self.Resolve(onSuccess, problem => problem switch
        {
            TP1 p1 => onProblem1(p1),
            _ => fallback(problem)
        });
    }

    /// <summary>
    /// Operation to produce a final value from this outcome.
    /// </summary>
    /// <typeparam name="TSource">The type of the source outcome value.</typeparam>
    /// <typeparam name="TResult">The type of the result outcome value.</typeparam>
    /// <typeparam name="TP1">The type of the first strong-typed problem resolver.</typeparam>
    /// <typeparam name="TP2">The type of the second strong-typed problem resolver.</typeparam>
    /// <param name="self">The source outcome on which to operate.</param>
    /// <param name="onSuccess">Transformation function to apply when there is no problem.</param>
    /// <param name="fallback">Transformation function to apply when there is a problem not matched by a strong-typed resolver.</param>
    /// <param name="onProblem1">Problem resolver for problems of type <typeparam name="TP1"/>.</param>
    /// <param name="onProblem2">Problem resolver for problems of type <typeparam name="TP2"/>.</param>
    /// <returns>The result of applying one of the transformation functions.</returns>
    /// <exception cref="ArgumentNullException">Thrown when either onSuccess or any problem resolver is null.</exception>
    public static TResult Resolve<TSource, TP1, TP2, TResult>(
        this Outcome<TSource> self,
        Func<TSource, TResult> onSuccess,
        Func<IProblem, TResult> fallback,
        Func<TP1, TResult> onProblem1,
        Func<TP2, TResult> onProblem2)
    {
        if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
        if (fallback == null) throw new ArgumentNullException(nameof(fallback));
        if (onProblem1 == null) throw new ArgumentNullException(nameof(onProblem1));
        if (onProblem2 == null) throw new ArgumentNullException(nameof(onProblem2));

        return self.Resolve(onSuccess, problem => problem switch
        {
            TP1 p1 => onProblem1(p1),
            TP2 p2 => onProblem2(p2),
            _ => fallback(problem)
        });
    }

    /// <summary>
    /// Operation to produce a final value from this outcome.
    /// </summary>
    /// <typeparam name="TSource">The type of the source outcome value.</typeparam>
    /// <typeparam name="TResult">The type of the result outcome value.</typeparam>
    /// <typeparam name="TP1">The type of the first strong-typed problem resolver.</typeparam>
    /// <typeparam name="TP2">The type of the second strong-typed problem resolver.</typeparam>
    /// <typeparam name="TP3">The type of the third strong-typed problem resolver.</typeparam>
    /// <param name="self">The source outcome on which to operate.</param>
    /// <param name="onSuccess">Transformation function to apply when there is no problem.</param>
    /// <param name="fallback">Transformation function to apply when there is a problem not matched by a strong-typed resolver.</param>
    /// <param name="onProblem1">Problem resolver for problems of type <typeparam name="TP1"/>.</param>
    /// <param name="onProblem2">Problem resolver for problems of type <typeparam name="TP2"/>.</param>
    /// <param name="onProblem3">Problem resolver for problems of type <typeparam name="TP3"/>.</param>
    /// <returns>The result of applying one of the transformation functions.</returns>
    /// <exception cref="ArgumentNullException">Thrown when either onSuccess or any problem resolver is null.</exception>
    public static TResult Resolve<TSource, TP1, TP2, TP3, TResult>(
        this Outcome<TSource> self,
        Func<TSource, TResult> onSuccess,
        Func<IProblem, TResult> fallback,
        Func<TP1, TResult> onProblem1,
        Func<TP2, TResult> onProblem2,
        Func<TP3, TResult> onProblem3)
    {
        if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
        if (fallback == null) throw new ArgumentNullException(nameof(fallback));
        if (onProblem1 == null) throw new ArgumentNullException(nameof(onProblem1));
        if (onProblem2 == null) throw new ArgumentNullException(nameof(onProblem2));
        if (onProblem3 == null) throw new ArgumentNullException(nameof(onProblem3));

        return self.Resolve(onSuccess, problem => problem switch
        {
            TP1 p1 => onProblem1(p1),
            TP2 p2 => onProblem2(p2),
            TP3 p3 => onProblem3(p3),
            _ => fallback(problem)
        });
    }

    /// <summary>
    /// Operation to produce a final value from this outcome.
    /// </summary>
    /// <typeparam name="TSource">The type of the source outcome value.</typeparam>
    /// <typeparam name="TResult">The type of the result outcome value.</typeparam>
    /// <typeparam name="TP1">The type of the first strong-typed problem resolver.</typeparam>
    /// <typeparam name="TP2">The type of the second strong-typed problem resolver.</typeparam>
    /// <typeparam name="TP3">The type of the third strong-typed problem resolver.</typeparam>
    /// <typeparam name="TP4">The type of the fourth strong-typed problem resolver.</typeparam>
    /// <param name="self">The source outcome on which to operate.</param>
    /// <param name="onSuccess">Transformation function to apply when there is no problem.</param>
    /// <param name="fallback">Transformation function to apply when there is a problem not matched by a strong-typed resolver.</param>
    /// <param name="onProblem1">Problem resolver for problems of type <typeparam name="TP1"/>.</param>
    /// <param name="onProblem2">Problem resolver for problems of type <typeparam name="TP2"/>.</param>
    /// <param name="onProblem3">Problem resolver for problems of type <typeparam name="TP3"/>.</param>
    /// <param name="onProblem4">Problem resolver for problems of type <typeparam name="TP4"/>.</param>
    /// <returns>The result of applying one of the transformation functions.</returns>
    /// <exception cref="ArgumentNullException">Thrown when either onSuccess or any problem resolver is null.</exception>
    public static TResult Resolve<TSource, TP1, TP2, TP3, TP4, TResult>(
        this Outcome<TSource> self,
        Func<TSource, TResult> onSuccess,
        Func<IProblem, TResult> fallback,
        Func<TP1, TResult> onProblem1,
        Func<TP2, TResult> onProblem2,
        Func<TP3, TResult> onProblem3,
        Func<TP4, TResult> onProblem4)
    {
        if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
        if (fallback == null) throw new ArgumentNullException(nameof(fallback));
        if (onProblem1 == null) throw new ArgumentNullException(nameof(onProblem1));
        if (onProblem2 == null) throw new ArgumentNullException(nameof(onProblem2));
        if (onProblem3 == null) throw new ArgumentNullException(nameof(onProblem3));
        if (onProblem4 == null) throw new ArgumentNullException(nameof(onProblem4));

        return self.Resolve(onSuccess, problem => problem switch
        {
            TP1 p1 => onProblem1(p1),
            TP2 p2 => onProblem2(p2),
            TP3 p3 => onProblem3(p3),
            TP4 p4 => onProblem4(p4),
            _ => fallback(problem)
        });
    }
}
