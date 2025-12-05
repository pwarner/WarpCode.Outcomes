namespace WarpCode.Outcomes;

/// <summary>
/// A function called when exceptions are thrown by code instead of returning outcomes.
/// If the function returns a <see cref="IProblem"/> then a new <see cref="Outcome{T}"/>
/// will be returned by the adaptive methods. If null is returned, the exception will be re-thrown.
/// </summary>
/// <param name="exception">A caught <see cref="Exception"/> instance to try to map to a <see cref="IProblem"/>.</param>
/// <returns>A <see cref="IProblem"/> if one could be created from the exception, or null.</returns>
public delegate IProblem? ExceptionMap(Exception exception);

/// <summary>
/// Strongly typed version of <see cref="ExceptionMap"/> allowing more concise mapping
/// of methods where only one Exception type needs to be handled.
/// </summary>
/// <typeparam name="TException">The single exception type that this delegate handles.</typeparam>
/// <param name="exception">A caught <see cref="Exception"/> instance to try to map to a <see cref="IProblem"/>.</param>
/// <returns>A <see cref="IProblem"/> if one could be created from the exception, or null.</returns>>
public delegate IProblem ExceptionMap<in TException>(TException exception) where TException : Exception;

/// <summary>
/// static root class for all adaptive extensions.
/// </summary>
public static class Adapt
{
    /// <summary>
    /// Optional global mapper.
    /// If provided, will be used by adaptive methods when their optional map parameters are null.
    /// </summary>
    public static ExceptionMap? MapExceptions { get; set; }

    extension<T>(Func<T> func)
    {
        /// <summary>
        /// Adapts a <see cref="Func{TResult}"/>.
        /// </summary>
        /// <param name="map">Optional <see cref="ExceptionMap"/> function.</param>
        /// <returns>An <see cref="Outcome{T}"/> carrying the function result, or a <see cref="IProblem"/>.</returns>
        /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
        public Outcome<T> ToOutcome(ExceptionMap? map = null)
        {
            map ??= MapExceptions;

            try
            {
                return func();
            }
            catch (Exception e)
            {
                IProblem? problem = map?.Invoke(e);
                if (problem is null) throw;
                return problem.ToOutcome<T>();
            }
        }

        /// <summary>
        /// Adapts a <see cref="Func{TResult}"/> for a single, strongly-typed exception.
        /// </summary>
        /// <typeparam name="TException">Type of exception being handled.</typeparam>
        /// <param name="map">A strongly-typed <see cref="ExceptionMap{TException}"/> function.</param>
        /// <returns>An <see cref="Outcome{T}"/> carrying the function result, or a <see cref="IProblem"/>.</returns>
        /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
        public Outcome<T> ToOutcome<TException>(ExceptionMap<TException>? map = null)
            where TException : Exception =>
            func.ToOutcome(NonGeneric(map));
    }

    extension(Action action)
    {
        /// <summary>
        /// Adapts an <see cref="Action"/>.
        /// </summary>
        /// <param name="map">Optional <see cref="ExceptionMap"/> function.</param>
        /// <returns>An <see cref="Outcome{None}"/> if the action completed, or a <see cref="Problem"/>.</returns>
        /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
        public Outcome<None> ToOutcome(ExceptionMap? map = null)
        {
            map ??= MapExceptions;

            try
            {
                action.Invoke();
                return Outcome.Ok;
            }
            catch (Exception e)
            {
                IProblem? problem = map?.Invoke(e);
                if (problem is null) throw;
                return problem.ToOutcome();
            }
        }

        /// <summary>
        /// Adapts an <see cref="Action"/> for a single, strongly-typed exception.
        /// </summary>
        /// <typeparam name="TException">Type of exception being handled.</typeparam>
        /// <param name="map">A strongly-typed <see cref="ExceptionMap{TException}"/> function.</param>
        /// <returns>An <see cref="Outcome{None}"/> if the action completed, or a <see cref="Problem"/>.</returns>
        /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
        public Outcome<None> ToOutcome<TException>(ExceptionMap<TException> map)
            where TException : Exception =>
            action.ToOutcome(NonGeneric(map));
    }

    extension<T>(Task<T> task)
    {
        /// <summary>
        /// Adapts a <see cref="Task{T}"/>.
        /// </summary>
        /// <param name="map">Optional <see cref="ExceptionMap"/> function.</param>
        /// <returns>A <see cref="Task{T}"/> that resolves to a <see cref="Outcome{T}"/>.</returns>
        /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
        public async Task<Outcome<T>> ToOutcome(ExceptionMap? map = null)
        {
            map ??= MapExceptions;

            try
            {
                return await task.ConfigureAwait(false);
            }
            catch (Exception e)
            {
                IProblem? problem = map?.Invoke(e);
                if (problem is null) throw;
                return problem.ToOutcome<T>();
            }
        }

        /// <summary>
        /// Adapts a <see cref="Task{T}"/> for a single, strongly-typed exception.
        /// </summary>
        /// <typeparam name="TException">Type of exception being handled.</typeparam>
        /// <param name="map">A strongly-typed <see cref="ExceptionMap{TException}"/> function.</param>
        /// <returns>A <see cref="Task{T}"/> that resolves to a <see cref="Outcome{T}"/>.</returns>
        /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
        public Task<Outcome<T>> ToOutcome<TException>(ExceptionMap<TException> map)
            where TException : Exception =>
            task.ToOutcome(NonGeneric(map));
    }

    extension(Task task)
    {
        /// <summary>
        /// Adapts a <see cref="Task"/>.
        /// </summary>
        /// <param name="map">Optional <see cref="ExceptionMap"/> function.</param>
        /// <returns>A <see cref="Task{T}"/> that resolves to an <see cref="Outcome{T}"/>.</returns>
        /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
        public async Task<Outcome<None>> ToOutcome(ExceptionMap? map = null)
        {
            map ??= MapExceptions;

            try
            {
                await task.ConfigureAwait(false);
                return Outcome.Ok;
            }
            catch (Exception e)
            {
                IProblem? problem = map?.Invoke(e);
                if (problem is null) throw;
                return problem.ToOutcome();
            }
        }

        /// <summary>
        /// Adapts a <see cref="Task"/> for a single, strongly-typed exception.
        /// </summary>
        /// <typeparam name="TException">Type of exception being handled.</typeparam>
        /// <param name="map">A strongly-typed <see cref="ExceptionMap{TException}"/> function.</param>
        /// <returns>A <see cref="Task{T}"/> that resolves to a value-less <see cref="Outcome{None}"/>.</returns>
        /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
        public Task<Outcome<None>> ToOutcome<TException>(ExceptionMap<TException> map)
            where TException : Exception =>
            task.ToOutcome(NonGeneric(map));
    }

    extension<T>(ValueTask<T> task)
    {
        /// <summary>
        /// Adapts a <see cref="ValueTask{T}"/>.
        /// </summary>
        /// <param name="map">Optional <see cref="ExceptionMap"/> function.</param>
        /// <returns>A <see cref="Task{T}"/> that resoves to a <see cref="Outcome{T}"/>.</returns>
        /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
        public async ValueTask<Outcome<T>> ToOutcome(ExceptionMap? map = null) =>
            await task.AsTask().ToOutcome(map).ConfigureAwait(false);

        /// <summary>
        /// Adapts a <see cref="ValueTask{T}"/> for a single, strongly-typed exception.
        /// </summary>
        /// <typeparam name="TException">Type of exception being handled.</typeparam>
        /// <param name="map">A strongly-typed <see cref="ExceptionMap{TException}"/> function.</param>
        /// <returns>A <see cref="Task{T}"/> that resolves to a <see cref="Outcome{T}"/>.</returns>
        /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
        public ValueTask<Outcome<T>> ToOutcome<TException>(ExceptionMap<TException> map)
            where TException : Exception =>
            task.ToOutcome(NonGeneric(map));
    }

    extension(ValueTask task)
    {
        /// <summary>
        /// Adapts a <see cref="ValueTask"/>.
        /// </summary>
        /// <param name="map">Optional <see cref="ExceptionMap"/> function.</param>
        /// <returns>A <see cref="Task{T}"/> that resolves to a <see cref="Outcome{None}"/>.</returns>
        /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
        public async ValueTask<Outcome<None>> ToOutcome(ExceptionMap? map = null) =>
            await task.AsTask().ToOutcome(map).ConfigureAwait(false);

        /// <summary>
        /// Adapts a <see cref="ValueTask"/> for a single, strongly-typed exception.
        /// </summary>
        /// <typeparam name="TException">Type of exception being handled.</typeparam>
        /// <param name="map">A strongly-typed <see cref="ExceptionMap{TException}"/> function.</param>
        /// <returns>A <see cref="Task{T}"/> that resolves to a value-less <see cref="Outcome{None}"/>.</returns>
        /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
        public ValueTask<Outcome<None>> ToOutcome<TException>(ExceptionMap<TException> map)
            where TException : Exception =>
            task.ToOutcome(NonGeneric(map));
    }

    private static ExceptionMap? NonGeneric<TException>(ExceptionMap<TException>? map)
        where TException : Exception =>
        map is null ? null : exception => exception switch
        {
            TException te => map.Invoke(te),
            _ => null
        };
}
