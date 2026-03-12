namespace WarpCode.Outcomes;

internal static class FunctionAdaptation
{
    internal static Func<T, ValueTask<Result<TNext>>> AsValueTask<T, TNext>(Func<T, Task<Result<TNext>>> bind)
        => async value => await bind(value);

    internal static Func<T, ValueTask<TNext>> AsValueTask<T, TNext>(Func<T, Task<TNext>> map)
        => async value => await map(value);

    internal static Func<Problem, ValueTask<Result<TNext>>> AsValueTask<TNext>(Func<Problem, Task<Result<TNext>>> rescue)
        => async value => await rescue(value);

    internal static Func<ValueTask<Result<TNext>>> AsValueTask<TNext>(Func<Task<Result<TNext>>> bind)
        => async () => await bind();

    internal static Func<ValueTask<TNext>> AsValueTask<TNext>(Func<Task<TNext>> map)
        => async () => await map();

    internal static Func<T, Result<T>> Wrap<T>(Action<T> onValue) => value =>
    {
        onValue(value);
        return new(value);
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

    internal static Func<Result<None>> Wrap(Action onValue) => () =>
    {
        onValue();
        return Result.Ok;
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
