namespace WarpCode.Outcomes;

/// <summary>
/// `OnProblem` composition extensions for async outcomes
/// </summary>
public static class AsyncOnProblemExtensions
{
    /// <param name="self">Current async outcome.</param>
    extension<T>(AsyncOutcome<T> self)
    {
        /// <summary>
        /// Wraps an async call to the `OnProblem` extension on the underlying outcome.
        /// </summary>
        private async ValueTask<Outcome<T>> Local(Action<Problem> onProblem)
            => (await self).OnProblem(onProblem);

        /// <summary>
        /// Async sad-path composition operator.
        /// If the async outcome resolves to a problem, the onProblem action is executed.
        /// If the async outcome resolves to a value, the onProblem action is not executed and the current outcome is returned.
        /// </summary>
        /// <param name="onProblem">Action to execute if the outcome is a problem.</param>
        /// <returns>The current outcome.</returns>
        public AsyncOutcome<T> OnProblem(Action<Problem> onProblem)
            => self.With(self.Local(onProblem));
    }
}