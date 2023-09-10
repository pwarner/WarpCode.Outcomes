namespace Outcomes;

/// <summary>
/// A function called when exceptions are thrown by code instead of returning outcomes.
/// If the function returns a <see cref="IProblem"/> then a new <see cref="Outcome{T}"/> or <see cref="AsyncOutcome{T}"/>
/// will be returned by the adaptive methods. If null is returned, the exception will be re-thrown.
/// </summary>
/// <param name="exception">A caught <see cref="Exception"/> instance to try to map to a <see cref="IProblem"/>.</param>
/// <returns>A <see cref="IProblem"/> if one could be created from the exception, or null.</returns>
public delegate IProblem? ExceptionMap(Exception exception);

public static class Adapt
{
    /// <summary>
    /// Optional global mapper.
    /// If provided, will be used by adaptive methods when their optional map parameters are null.
    /// </summary>
    public static ExceptionMap? MapExceptions { get; set; }

    /// <summary>
    /// Adapts a function that does not return a <see cref="Outcome{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of return value from the function.</typeparam>
    /// <param name="func">The function to adapt.</param>
    /// <param name="map">Optional <see cref="ExceptionMap"/> function.</param>
    /// <returns>An <see cref="Outcome{T}"/> carrying the function result, or a <see cref="IProblem"/>.</returns>
    /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
    public static Outcome<T> ToOutcome<T>(Func<T> func, ExceptionMap? map = null)
    {
        try
        {
            return func();
        }
        catch (Exception e)
        {
            IProblem? problem = (map ?? MapExceptions)?.Invoke(e);
            if (problem is null) throw;
            return problem.ToOutcome();
        }
    }

    /// <summary>
    /// Adapts an action that does not return a <see cref="Outcome{T}"/>.
    /// </summary>
    /// <param name="action">The action to adapt.</param>
    /// <param name="map">Optional <see cref="ExceptionMap"/> function.</param>
    /// <returns>An <see cref="Outcome{None}"/> if the action completed, or a <see cref="Problem"/>.</returns>
    /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
    public static Outcome<None> ToOutcome(Action action, ExceptionMap? map = null) =>
        ToOutcome<None>(() =>
        {
            action();
            return default;
        }, map);

    /// <summary>
    /// Extension method that adapts a <see cref="Task{T}"/>.
    /// </summary>
    /// <param name="task">The <see cref="Task{T}"/> to adapt.</param>
    /// <param name="map">Optional <see cref="ExceptionMap"/> function.</param>
    /// <returns>An <see cref="Task{T}"/> that yields an <see cref="Outcome{T}"/>.</returns>
    /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
    public static async Task<Outcome<T>> ToOutcome<T>(
        this Task<T> task,
        ExceptionMap? map = null)
    {
        try
        {
            return await task.ConfigureAwait(false);
        }
        catch (Exception e)
        {
            IProblem? problem = (map ?? MapExceptions)?.Invoke(e);
            if (problem is null) throw;
            return problem.ToOutcome();
        }
    }

    /// <summary>
    /// Extension method that adapts a <see cref="Task"/>.
    /// </summary>
    /// <param name="task">The <see cref="Task"/> to adapt.</param>
    /// <param name="map">Optional <see cref="ExceptionMap"/> function.</param>
    /// <returns>An <see cref="Task{T}"/> that yields an <see cref="Outcome{None}"/>.</returns>
    /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
    public static async Task<Outcome<None>> ToOutcome(
        this Task task,
        ExceptionMap? map = null)
    {
        try
        {
            await task.ConfigureAwait(false);
            return Outcome.Ok();
        }
        catch (Exception e)
        {
            IProblem? problem = (map ?? MapExceptions)?.Invoke(e);
            if (problem is null) throw;
            return problem.ToOutcome();
        }
    }

    /// <summary>
    /// Extension method that adapts a <see cref="ValueTask{T}"/>.
    /// </summary>
    /// <param name="task">The <see cref="ValueTask{T}"/> to adapt.</param>
    /// <param name="map">Optional <see cref="ExceptionMap"/> function.</param>
    /// <returns>An <see cref="Task{T}"/> that yields an <see cref="Outcome{T}"/>.</returns>
    /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
    public static async Task<Outcome<T>> ToOutcome<T>(
        this ValueTask<T> task,
        ExceptionMap? map = null)
    {
        try
        {
            return await task.ConfigureAwait(false);
        }
        catch (Exception e)
        {
            IProblem? problem = (map ?? MapExceptions)?.Invoke(e);
            if (problem is null) throw;
            return problem.ToOutcome();
        }
    }

    /// <summary>
    /// Extension method that adapts a <see cref="ValueTask"/>.
    /// </summary>
    /// <param name="task">The <see cref="ValueTask"/> to adapt.</param>
    /// <param name="map">Optional <see cref="ExceptionMap"/> function.</param>
    /// <returns>An <see cref="Task{T}"/> that yields an <see cref="Outcome{None}"/>.</returns>
    /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
    public static async Task<Outcome<None>> ToOutcome(
        this ValueTask task,
        ExceptionMap? map = null)
    {
        try
        {
            await task.ConfigureAwait(false);
            return Outcome.Ok();
        }
        catch (Exception e)
        {
            IProblem? problem = (map ?? MapExceptions)?.Invoke(e);
            if (problem is null) throw;
            return problem.ToOutcome();
        }
    }
}
