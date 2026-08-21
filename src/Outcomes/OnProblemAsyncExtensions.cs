namespace WarpCode.Outcomes;

/// <summary>
/// `OnProblem` composition extensions for async outcomes that take an asynchronous delegate.
/// </summary>
/// <remarks>
/// Two families of overloads are provided: <c>OnProblemAsync</c> for delegates returning a <see cref="System.Threading.Tasks.ValueTask"/> (the preferred, allocation-friendly form and the natural target for inline <c>async</c> lambdas) and <c>OnProblemTask</c> for delegates returning a <see cref="System.Threading.Tasks.Task"/> (so an existing <c>Task</c>-returning method can be composed directly, including as a method group). The names are kept distinct rather than overloaded on return type because an <c>async</c> lambda is convertible to both, which would otherwise make every inline <c>async</c> lambda ambiguous (CS0121). A <see cref="CancellationToken"/> is supplied as the last delegate parameter.
/// </remarks>
public static class OnProblemAsyncExtensions
{
    /// <param name="self">Current async outcome.</param>
    extension<T>(AsyncOutcome<T> self)
    {
        /// <summary>
        /// Wraps an async call to the `OnProblem` operator on the underlying outcome.
        /// </summary>
        private async ValueTask<Outcome<T>> Local(Func<Problem, CancellationToken, ValueTask> onProblem)
        {
            Outcome<T> outcome = await self;
            if (outcome.Problem is { } problem)
                await onProblem(problem, self.CancellationToken);
            return outcome;
        }

        /// <summary>
        /// Async sad-path composition operator.
        /// If the async outcome resolves to a problem, the onProblem action is executed.
        /// If the async outcome resolves to a value, the onProblem action is not executed and the current outcome is returned.
        /// </summary>
        /// <param name="onProblem">Action to execute if the outcome is a problem.</param>
        /// <returns>The current outcome.</returns>
        public AsyncOutcome<T> OnProblemAsync(Func<Problem, CancellationToken, ValueTask> onProblem)
            => self.With(self.Local(onProblem));

        /// <summary>
        /// `Task`-returning counterpart of <see cref="OnProblemAsyncExtensions.extension{T}(AsyncOutcome{T}).OnProblemAsync(Func{Problem, CancellationToken, ValueTask})"/>.
        /// Prefer the <c>OnProblemAsync</c> (ValueTask) form for inline <c>async</c> lambdas; use this to compose an existing <c>Task</c>-returning method directly.
        /// </summary>
        /// <param name="onProblem">Action to execute if the outcome is a problem.</param>
        /// <returns>The current outcome.</returns>
        public AsyncOutcome<T> OnProblemTask(Func<Problem, CancellationToken, Task> onProblem)
            => self.OnProblemAsync((p, c) => new ValueTask(onProblem(p, c)));
    }
}
