namespace WarpCode.Outcomes;

internal static class FunctionAdaptation
{
    internal static Func<T, Result<TNext>> Wrap<T, TNext>(Func<T, TNext> next)
        => value => new(next(value));

    internal static Func<Result<TNext>> Wrap<TNext>(Func<TNext> next)
        => () => new(next());

    internal static Func<T, ValueTask<Result<TNext>>> WrapAsync<T, TNext>(Func<T, Result<TNext>> next)
        => value => ValueTask.FromResult(next(value));

    internal static Func<T, ValueTask<Result<TNext>>> WrapAsync<T, TNext>(Func<T, TNext> next)
        => value => ValueTask.FromResult<Result<TNext>>(new(next(value)));

    internal static Func<T, ValueTask<Result<TNext>>> WrapAsync<T, TNext>(Func<T, ValueTask<TNext>> next)
        => async value => new(await next(value));

    internal static Func<T, ValueTask<Result<TNext>>> WrapAsync<T, TNext>(Func<T, Task<TNext>> next)
        => async value => new(await next(value));

    internal static Func<T, ValueTask<Result<TNext>>> WrapAsync<T, TNext>(Func<T, Task<Result<TNext>>> next)
        => async value => await next(value);

    internal static Func<ValueTask<Result<TNext>>> WrapAsync<TNext>(Func<Task<Result<TNext>>> next)
        => async () => await next();

    internal static Func<ValueTask<Result<TNext>>> WrapAsync<TNext>(Func<ValueTask<TNext>> next)
        => async () => new(await next());

    internal static Func<ValueTask<Result<TNext>>> WrapAsync<TNext>(Func<Task<TNext>> next)
        => async () => new(await next());

    internal static Func<T, Result<T>> Wrap<T>(Action<T> onValue) => value =>
    {
        onValue(value);
        return new(value);
    };

    internal static Func<Result<None>> Wrap(Action onValue) => () =>
    {
        onValue();
        return Result.Ok;
    };

    internal static Func<T, ValueTask<Result<T>>> WrapAsync<T>(Func<T, Task> onValue) => async value =>
    {
        await onValue(value);
        return new(value);
    };

    internal static Func<T, ValueTask<Result<T>>> WrapAsync<T>(Func<T, ValueTask> onValue) => async value =>
    {
        await onValue(value);
        return new(value);
    };

    internal static Func<ValueTask<Result<None>>> WrapAsync(Func<Task> onValue) => async () =>
    {
        await onValue();
        return Result.Ok;
    };

    internal static Func<ValueTask<Result<None>>> WrapAsync(Func<ValueTask> onValue) => async () =>
    {
        await onValue();
        return Result.Ok;
    };

    internal static Func<Problem, Result<T>> Wrap<T>(Action<Problem> onProblem) => problem =>
    {
        onProblem(problem);
        return new(problem);
    };

    internal static Func<Problem, ValueTask<Result<T>>> WrapAsync<T>(Func<Problem, Task> onProblem) => async problem =>
    {
        await onProblem(problem);
        return new(problem);
    };

    internal static Func<Problem, ValueTask<Result<T>>> WrapAsync<T>(Func<Problem, ValueTask> onProblem) => async problem =>
    {
        await onProblem(problem);
        return new(problem);
    };
}
