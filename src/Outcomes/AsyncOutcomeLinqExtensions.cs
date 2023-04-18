namespace Outcomes;

public static class AsyncOutcomeLinqExtensions
{
    /// <summary>
    /// Syntactic operator to support the 'select' clause in a LINQ natural query comprehension.
    /// Creates a new <see cref="AsyncOutcome{T}"/> by transforming the eventual value of the source outcome if it holds no <see cref="Problem"/>.
    /// If the outcome holds a problem, the new outcome will carry the same problem.
    /// </summary>
    /// <typeparam name="TSource">The type of the source outcome value.</typeparam>
    /// <typeparam name="TResult">The type of the result outcome value.</typeparam>
    /// <param name="self">The source outcome on which to operate.</param>
    /// <param name="selector">A transform function to apply to the outcome value.</param>
    /// <returns>An <see cref="AsyncOutcome{TResult}"/> whose eventual value is either the result of invoking the transform function on the value of source,
    /// or a <see cref="Problem"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the selector function is null.</exception>
    public static AsyncOutcome<TResult> Select<TSource, TResult>(
        this in AsyncOutcome<TSource> self,
        Func<TSource, TResult> selector)
    {
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        return self.Then(x => new AsyncOutcome<TResult>(selector(x)));
    }

    /// <summary>
    /// Syntactic operator to support multiple 'from' clauses in a LINQ natural query comprehension
    /// where the subsequent 'from' clause returns an <see cref="Outcome{TNext}"/>.
    /// Creates a new <see cref="AsyncOutcome{TResult}"/> by invoking selctor and projector functions.
    /// TResult will be a compiler-provided type combining both TSource and TNext values.
    /// If either outcome holds a <see cref="Problem"/>, so will the result.
    /// </summary>
    /// <typeparam name="TSource">The type of the source outcome value.</typeparam>
    /// <typeparam name="TNext">The type of the next outcome value.</typeparam>
    /// <typeparam name="TResult">The type of the combined scope outcome value.</typeparam>
    /// <param name="self">The source outcome on which to operate.</param>
    /// <param name="selector">A compiler-provided function to obtain the next <see cref="Outcome{TNext}"/> by invoking the selector function with the outcome value.</param>
    /// <param name="projector">A compiler-provided function to produce a tuple representing the combined values.</param>
    /// <returns>An <see cref="AsyncOutcome{TResult}"/> whose eventual value is either the result of invoking the composition functions on the value of source, or a <see cref="Problem"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the selector or projector functions are null.</exception>
    public static AsyncOutcome<TResult> SelectMany<TSource, TNext, TResult>(
        this in AsyncOutcome<TSource> self,
        Func<TSource, Outcome<TNext>> selector,
        Func<TSource, TNext, TResult> projector)
    {
        if (selector is null) throw new ArgumentNullException(nameof(selector));
        if (projector is null) throw new ArgumentNullException(nameof(projector));

        return self.SelectMany(
            t => new AsyncOutcome<TNext>(selector(t)),
            projector);
    }

    /// <summary>
    /// Syntactic operator to support multiple 'from' clauses in a LINQ natural query comprehension
    /// where the subsequent 'from' clause returns a <see cref="Task{T}"/> of an <see cref="Outcome{TNext}"/>.
    /// Creates a new <see cref="AsyncOutcome{TResult}"/> by invoking selctor and projector functions.
    /// TResult will be a compiler-provided type combining both TSource and TNext values.
    /// If either outcome holds a <see cref="Problem"/>, so will the result.
    /// </summary>
    /// <typeparam name="TSource">The type of the source outcome value.</typeparam>
    /// <typeparam name="TNext">The type of the next outcome value.</typeparam>
    /// <typeparam name="TResult">The type of the combined scope outcome value.</typeparam>
    /// <param name="self">The source outcome on which to operate.</param>
    /// <param name="selector">A compiler-provided function to obtain the next <see cref="AsyncOutcome{TNext}"/> by invoking the selector function with the outcome value.</param>
    /// <param name="projector">A compiler-provided function to produce a tuple holding the combined values.</param>
    /// <returns>An <see cref="AsyncOutcome{TResult}"/> whose eventual value is either the result of invoking the composition functions on the value of source, or a <see cref="Problem"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the selector or projector functions are null.</exception>
    public static AsyncOutcome<TResult> SelectMany<TSource, TNext, TResult>(
        this in AsyncOutcome<TSource> self,
        Func<TSource, Task<Outcome<TNext>>> selector,
        Func<TSource, TNext, TResult> projector)
    {
        if (selector is null) throw new ArgumentNullException(nameof(selector));
        if (projector is null) throw new ArgumentNullException(nameof(projector));

        return self.SelectMany(
            t => new AsyncOutcome<TNext>(selector(t)),
            projector);
    }

    /// <summary>
    /// Syntactic operator to support multiple 'from' clauses in a LINQ natural query comprehension
    /// where the subsequent 'from' clause returns a <see cref="ValueTask{T}"/> of an <see cref="Outcome{TNext}"/>.
    /// Creates a new <see cref="AsyncOutcome{TResult}"/> by invoking selctor and projector functions.
    /// TResult will be a compiler-provided type combining both TSource and TNext values.
    /// If either outcome holds a <see cref="Problem"/>, so will the result.
    /// </summary>
    /// <typeparam name="TSource">The type of the source outcome value.</typeparam>
    /// <typeparam name="TNext">The type of the next outcome value.</typeparam>
    /// <typeparam name="TResult">The type of the combined scope outcome value.</typeparam>
    /// <param name="self">The source outcome on which to operate.</param>
    /// <param name="selector">A compiler-provided function to obtain the next <see cref="AsyncOutcome{TNext}"/> by invoking the selector function with the outcome value.</param>
    /// <param name="projector">A compiler-provided function to produce a tuple holding the combined values.</param>
    /// <returns>An <see cref="AsyncOutcome{TResult}"/> whose eventual value is either the result of invoking the composition functions on the value of source, or a <see cref="Problem"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the selector or projector functions are null.</exception>
    public static AsyncOutcome<TResult> SelectMany<TSource, TNext, TResult>(
        this in AsyncOutcome<TSource> self,
        Func<TSource, ValueTask<Outcome<TNext>>> selector,
        Func<TSource, TNext, TResult> projector)
    {
        if (selector is null) throw new ArgumentNullException(nameof(selector));
        if (projector is null) throw new ArgumentNullException(nameof(projector));

        return self.SelectMany(
            t => new AsyncOutcome<TNext>(selector(t)),
            projector);
    }

    /// <summary>
    /// Syntactic operator to support multiple 'from' clauses in a LINQ natural query comprehension
    /// where the subsequent 'from' clause returns a <see cref="Task{TNext}"/>.
    /// Creates a new <see cref="AsyncOutcome{TResult}"/> by invoking selctor and projector functions.
    /// TResult will be a compiler-provided type combining both TSource and TNext values.
    /// If either outcome holds a <see cref="Problem"/>, so will the result.
    /// </summary>
    /// <typeparam name="TSource">The type of the source outcome value.</typeparam>
    /// <typeparam name="TNext">The type of the next outcome value.</typeparam>
    /// <typeparam name="TResult">The type of the combined scope outcome value.</typeparam>
    /// <param name="self">The source outcome on which to operate.</param>
    /// <param name="selector">A compiler-provided function to obtain the next <see cref="AsyncOutcome{TNext}"/> by invoking the selector function with the outcome value.</param>
    /// <param name="projector">A compiler-provided function to produce a tuple holding the combined values.</param>
    /// <returns>An <see cref="AsyncOutcome{TResult}"/> whose eventual value is either the result of invoking the composition functions on the value of source, or a <see cref="Problem"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the selector or projector functions are null.</exception>
    public static AsyncOutcome<TResult> SelectMany<TSource, TNext, TResult>(
        this in AsyncOutcome<TSource> self,
        Func<TSource, Task<TNext>> selector,
        Func<TSource, TNext, TResult> projector)
    {
        if (selector is null) throw new ArgumentNullException(nameof(selector));
        if (projector is null) throw new ArgumentNullException(nameof(projector));

        return self.SelectMany(
            t => selector(t).ToOutcome(),
            projector);
    }

    /// <summary>
    /// Syntactic operator to support multiple 'from' clauses in a LINQ natural query comprehension
    /// where the subsequent 'from' clause returns a <see cref="ValueTask{TNext}"/>.
    /// Creates a new <see cref="AsyncOutcome{TResult}"/> by invoking selctor and projector functions.
    /// TResult will be a compiler-provided type combining both TSource and TNext values.
    /// If either outcome holds a <see cref="Problem"/>, so will the result.
    /// </summary>
    /// <typeparam name="TSource">The type of the source outcome value.</typeparam>
    /// <typeparam name="TNext">The type of the next outcome value.</typeparam>
    /// <typeparam name="TResult">The type of the combined scope outcome value.</typeparam>
    /// <param name="self">The source outcome on which to operate.</param>
    /// <param name="selector">A compiler-provided function to obtain the next <see cref="AsyncOutcome{TNext}"/> by invoking the selector function with the outcome value.</param>
    /// <param name="projector">A compiler-provided function to produce a tuple holding the combined values.</param>
    /// <returns>An <see cref="AsyncOutcome{TResult}"/> whose eventual value is either the result of invoking the composition functions on the value of source, or a <see cref="Problem"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the selector or projector functions are null.</exception>
    public static AsyncOutcome<TResult> SelectMany<TSource, TNext, TResult>(
        this in AsyncOutcome<TSource> self,
        Func<TSource, ValueTask<TNext>> selector,
        Func<TSource, TNext, TResult> projector)
    {
        if (selector is null) throw new ArgumentNullException(nameof(selector));
        if (projector is null) throw new ArgumentNullException(nameof(projector));

        return self.SelectMany(
            t => selector(t).ToOutcome(),
            projector);
    }

    /// <summary>
    /// Syntactic operator to support multiple 'from' clauses in a LINQ natural query comprehension
    /// where the subsequent 'from' clause returns a <see cref="Task"/>.
    /// Creates a new <see cref="AsyncOutcome{TResult}"/> by invoking selctor and projector functions.
    /// TResult will be a compiler-provided type combining both TSource and <see cref="None"/> values.
    /// If either outcome holds a <see cref="Problem"/>, so will the result.
    /// </summary>
    /// <typeparam name="TSource">The type of the source outcome value.</typeparam>
    /// <typeparam name="TResult">The type of the combined scope outcome value.</typeparam>
    /// <param name="self">The source outcome on which to operate.</param>
    /// <param name="selector">A compiler-provided function to obtain the next <see cref="AsyncOutcome{TNext}"/> by invoking the selector function with the outcome value.</param>
    /// <param name="projector">A compiler-provided function to produce a tuple holding the combined values.</param>
    /// <returns>An <see cref="AsyncOutcome{TResult}"/> whose eventual value is either the result of invoking the composition functions on the value of source, or a <see cref="Problem"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the selector or projector functions are null.</exception>
    public static AsyncOutcome<TResult> SelectMany<TSource, TResult>(
        this in AsyncOutcome<TSource> self,
        Func<TSource, Task> selector,
        Func<TSource, None, TResult> projector)
    {
        if (selector is null) throw new ArgumentNullException(nameof(selector));
        if (projector is null) throw new ArgumentNullException(nameof(projector));

        return self.SelectMany(
            t => selector(t).ToOutcome(),
            projector);
    }

    /// <summary>
    /// Syntactic operator to support multiple 'from' clauses in a LINQ natural query comprehension
    /// where the subsequent 'from' clause returns a <see cref="ValueTask"/>.
    /// Creates a new <see cref="AsyncOutcome{TResult}"/> by invoking selctor and projector functions.
    /// TResult will be a compiler-provided type combining both TSource and <see cref="None"/> values.
    /// If either outcome holds a <see cref="Problem"/>, so will the result.
    /// </summary>
    /// <typeparam name="TSource">The type of the source outcome value.</typeparam>
    /// <typeparam name="TResult">The type of the combined scope outcome value.</typeparam>
    /// <param name="self">The source outcome on which to operate.</param>
    /// <param name="selector">A compiler-provided function to obtain the next <see cref="AsyncOutcome{TNext}"/> by invoking the selector function with the outcome value.</param>
    /// <param name="projector">A compiler-provided function to produce a tuple holding the combined values.</param>
    /// <returns>An <see cref="AsyncOutcome{TResult}"/> whose eventual value is either the result of invoking the composition functions on the value of source, or a <see cref="Problem"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the selector or projector functions are null.</exception>
    public static AsyncOutcome<TResult> SelectMany<TSource, TResult>(
        this in AsyncOutcome<TSource> self,
        Func<TSource, ValueTask> selector,
        Func<TSource, None, TResult> projector)
    {
        if (selector is null) throw new ArgumentNullException(nameof(selector));
        if (projector is null) throw new ArgumentNullException(nameof(projector));

        return self.SelectMany(
            t => selector(t).ToOutcome(),
            projector);
    }

    private static AsyncOutcome<TResult> SelectMany<TSource, TNext, TResult>(
        this in AsyncOutcome<TSource> self,
        Func<TSource, AsyncOutcome<TNext>> selector,
        Func<TSource, TNext, TResult> projector)
    {
        if (selector is null) throw new ArgumentNullException(nameof(selector));
        if (projector is null) throw new ArgumentNullException(nameof(projector));

        return self.Then(source =>
            from next in selector(source)
            select projector(source, next));
    }
}
