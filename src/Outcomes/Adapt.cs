namespace WarpCode.Outcomes;

/// <summary>
/// A function called when exceptions are thrown by code instead of returning results.
/// If the function returns a <see cref="Problem"/> then a new <see cref="Result{T}"/>
/// will be returned by the adaptive methods. If null is returned, the exception will be re-thrown.
/// </summary>
/// <param name="exception">A caught <see cref="Exception"/> instance to try to map to a <see cref="Problem"/>.</param>
/// <returns>A <see cref="Problem"/> if one could be created from the exception, or null.</returns>
public delegate Problem? ExceptionMap(Exception exception);

/// <summary>
/// Strongly typed version of <see cref="ExceptionMap"/> allowing more concise mapping
/// of methods where only one Exception type needs to be handled.
/// </summary>
/// <typeparam name="TException">The single exception type that this delegate handles.</typeparam>
/// <param name="exception">A caught <see cref="Exception"/> instance to try to map to a <see cref="Problem"/>.</param>
/// <returns>A <see cref="Problem"/> if one could be created from the exception, or null.</returns>>
public delegate Problem ExceptionMap<in TException>(TException exception) where TException : Exception;

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
        /// <returns>An <see cref="Result{T}"/> carrying the function result, or a <see cref="Problem"/>.</returns>
        /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
        public Result<T> ToResult(ExceptionMap? map = null)
        {
            map ??= MapExceptions;

            try
            {
                return func();
            }
            catch (Exception e)
            {
                Problem? problem = map?.Invoke(e);
                if (problem is null) throw;
                return new(problem);
            }
        }

        /// <summary>
        /// Adapts a <see cref="Func{TResult}"/> for a single, strongly-typed exception.
        /// </summary>
        /// <typeparam name="TException">Type of exception being handled.</typeparam>
        /// <param name="map">A strongly-typed <see cref="ExceptionMap{TException}"/> function.</param>
        /// <returns>An <see cref="Result{T}"/> carrying the function result, or a <see cref="Problem"/>.</returns>
        /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
        public Result<T> ToResult<TException>(ExceptionMap<TException>? map = null)
            where TException : Exception =>
            func.ToResult(NonGeneric(map));
    }

    extension(Action action)
    {
        /// <summary>
        /// Adapts an <see cref="Action"/>.
        /// </summary>
        /// <param name="map">Optional <see cref="ExceptionMap"/> function.</param>
        /// <returns>An <see cref="Result{None}"/> if the action completed, or a <see cref="Problem"/>.</returns>
        /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
        public Result<None> ToResult(ExceptionMap? map = null)
        {
            map ??= MapExceptions;

            try
            {
                action.Invoke();
                return Result.Ok;
            }
            catch (Exception e)
            {
                Problem? problem = map?.Invoke(e);
                if (problem is null) throw;
                return new(problem);
            }
        }

        /// <summary>
        /// Adapts an <see cref="Action"/> for a single, strongly-typed exception.
        /// </summary>
        /// <typeparam name="TException">Type of exception being handled.</typeparam>
        /// <param name="map">A strongly-typed <see cref="ExceptionMap{TException}"/> function.</param>
        /// <returns>An <see cref="Result{None}"/> if the action completed, or a <see cref="Problem"/>.</returns>
        /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
        public Result<None> ToResult<TException>(ExceptionMap<TException> map)
            where TException : Exception =>
            action.ToResult(NonGeneric(map));
    }

    extension<T>(Task<T> task)
    {
        /// <summary>
        /// Adapts a <see cref="Task{T}"/>.
        /// </summary>
        /// <param name="map">Optional <see cref="ExceptionMap"/> function.</param>
        /// <returns>A <see cref="Task{T}"/> that resolves to a <see cref="Result{T}"/>.</returns>
        /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
        public async Task<Result<T>> ToResult(ExceptionMap? map = null)
        {
            map ??= MapExceptions;

            try
            {
                return await task.ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Problem? problem = map?.Invoke(e);
                if (problem is null) throw;
                return new(problem);
            }
        }

        /// <summary>
        /// Adapts a <see cref="Task{T}"/> for a single, strongly-typed exception.
        /// </summary>
        /// <typeparam name="TException">Type of exception being handled.</typeparam>
        /// <param name="map">A strongly-typed <see cref="ExceptionMap{TException}"/> function.</param>
        /// <returns>A <see cref="Task{T}"/> that resolves to a <see cref="Result{T}"/>.</returns>
        /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
        public Task<Result<T>> ToResult<TException>(ExceptionMap<TException> map)
            where TException : Exception =>
            task.ToResult(NonGeneric(map));
    }

    extension(Task task)
    {
        /// <summary>
        /// Adapts a <see cref="Task"/>.
        /// </summary>
        /// <param name="map">Optional <see cref="ExceptionMap"/> function.</param>
        /// <returns>A <see cref="Task{T}"/> that resolves to an <see cref="Result{T}"/>.</returns>
        /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
        public async Task<Result<None>> ToResult(ExceptionMap? map = null)
        {
            map ??= MapExceptions;

            try
            {
                await task.ConfigureAwait(false);
                return Result.Ok;
            }
            catch (Exception e)
            {
                Problem? problem = map?.Invoke(e);
                if (problem is null) throw;
                return new(problem);
            }
        }

        /// <summary>
        /// Adapts a <see cref="Task"/> for a single, strongly-typed exception.
        /// </summary>
        /// <typeparam name="TException">Type of exception being handled.</typeparam>
        /// <param name="map">A strongly-typed <see cref="ExceptionMap{TException}"/> function.</param>
        /// <returns>A <see cref="Task{T}"/> that resolves to a value-less <see cref="Result{None}"/>.</returns>
        /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
        public Task<Result<None>> ToResult<TException>(ExceptionMap<TException> map)
            where TException : Exception =>
            task.ToResult(NonGeneric(map));
    }

    extension<T>(ValueTask<T> task)
    {
        /// <summary>
        /// Adapts a <see cref="ValueTask{T}"/>.
        /// </summary>
        /// <param name="map">Optional <see cref="ExceptionMap"/> function.</param>
        /// <returns>A <see cref="Task{T}"/> that resoves to a <see cref="Result{T}"/>.</returns>
        /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
        public async ValueTask<Result<T>> ToResult(ExceptionMap? map = null) =>
            await task.AsTask().ToResult(map).ConfigureAwait(false);

        /// <summary>
        /// Adapts a <see cref="ValueTask{T}"/> for a single, strongly-typed exception.
        /// </summary>
        /// <typeparam name="TException">Type of exception being handled.</typeparam>
        /// <param name="map">A strongly-typed <see cref="ExceptionMap{TException}"/> function.</param>
        /// <returns>A <see cref="Task{T}"/> that resolves to a <see cref="Result{T}"/>.</returns>
        /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
        public ValueTask<Result<T>> ToResult<TException>(ExceptionMap<TException> map)
            where TException : Exception =>
            task.ToResult(NonGeneric(map));
    }

    extension(ValueTask task)
    {
        /// <summary>
        /// Adapts a <see cref="ValueTask"/>.
        /// </summary>
        /// <param name="map">Optional <see cref="ExceptionMap"/> function.</param>
        /// <returns>A <see cref="Task{T}"/> that resolves to a <see cref="Result{None}"/>.</returns>
        /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
        public async ValueTask<Result<None>> ToResult(ExceptionMap? map = null) =>
            await task.AsTask().ToResult(map).ConfigureAwait(false);

        /// <summary>
        /// Adapts a <see cref="ValueTask"/> for a single, strongly-typed exception.
        /// </summary>
        /// <typeparam name="TException">Type of exception being handled.</typeparam>
        /// <param name="map">A strongly-typed <see cref="ExceptionMap{TException}"/> function.</param>
        /// <returns>A <see cref="Task{T}"/> that resolves to a value-less <see cref="Result{None}"/>.</returns>
        /// <exception cref="Exception">Re-throws any unmapped exceptions.</exception>
        public ValueTask<Result<None>> ToResult<TException>(ExceptionMap<TException> map)
            where TException : Exception =>
            task.ToResult(NonGeneric(map));
    }

    private static ExceptionMap? NonGeneric<TException>(ExceptionMap<TException>? map)
        where TException : Exception =>
        exception => exception switch
        {
            TException te => map?.Invoke(te),
            _ => null
        };
}
