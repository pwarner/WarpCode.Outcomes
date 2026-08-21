namespace WarpCode.Outcomes;

/// <summary>
/// `Rescue` composition extensions for async outcomes that take an asynchronous delegate.
/// </summary>
/// <remarks>
/// Two families of overloads are provided: <c>RescueAsync</c> for delegates returning a <see cref="System.Threading.Tasks.ValueTask{TResult}"/> (the preferred, allocation-friendly form and the natural target for inline <c>async</c> lambdas) and <c>RescueTask</c> for delegates returning a <see cref="System.Threading.Tasks.Task{TResult}"/> (so an existing <c>Task</c>-returning method can be composed directly, including as a method group). The names are kept distinct rather than overloaded on return type because an <c>async</c> lambda is convertible to both, which would otherwise make every inline <c>async</c> lambda ambiguous (CS0121). A <see cref="CancellationToken"/> is supplied as the last delegate parameter.
/// </remarks>
public static class RescueAsyncExtensions
{
    /// <param name="self">Current async outcome.</param>
    extension<T>(AsyncOutcome<T> self)
    {
        /// <summary>
        /// Wraps an async call to the `Rescue` operator on the underlying outcome.
        /// </summary>
        private async ValueTask<Outcome<T>> Local(Func<Problem, CancellationToken, ValueTask<Outcome<T>>> rescue)
            => (await self) switch
            {
                { Problem: { } problem } => await rescue(problem, self.CancellationToken),
                var outcome => outcome
            };

        /// <summary>
        /// Async sad-path composition operator.
        /// If the async outcome resolves to a problem, the rescue function is executed to attempt to recover from the problem and produce the next outcome.
        /// If the async outcome resolves to a value, the rescue function is not executed and the current outcome returned.
        /// </summary>
        /// <param name="rescue">Function to execute to attempt recovery from the problem.</param>
        /// <returns>The current async outcome if it resolves to a value, otherwise a new async outcome representing the result of the rescue function.</returns>
        public AsyncOutcome<T> RescueAsync(Func<Problem, CancellationToken, ValueTask<Outcome<T>>> rescue)
            => self.With(self.Local(rescue));

        /// <summary>
        /// `Task`-returning counterpart of <see cref="RescueAsyncExtensions.extension{T}(AsyncOutcome{T}).RescueAsync(Func{Problem, CancellationToken, ValueTask{Outcome{T}}})"/>.
        /// Prefer the <c>RescueAsync</c> (ValueTask) form for inline <c>async</c> lambdas; use this to compose an existing <c>Task</c>-returning method directly.
        /// </summary>
        /// <param name="rescue">Function to execute to attempt recovery from the problem.</param>
        /// <returns>The current async outcome if it resolves to a value, otherwise a new async outcome representing the result of the rescue function.</returns>
        public AsyncOutcome<T> RescueTask(Func<Problem, CancellationToken, Task<Outcome<T>>> rescue)
            => self.RescueAsync((p, c) => new ValueTask<Outcome<T>>(rescue(p, c)));
    }
}
