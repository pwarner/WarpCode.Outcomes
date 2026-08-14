namespace WarpCode.Outcomes;

/// <summary>
/// `Rescue` composition extensions for async outcomes
/// </summary>
public static class AsyncRescueExtensions
{
    /// <param name="self">Current async outcome.</param>
    extension<T>(AsyncOutcome<T> self)
    {
        /// <summary>
        /// Wraps an async call to the `Rescue` extension on the underlying outcome.
        /// </summary>
        private async ValueTask<Outcome<T>> Local(Func<Problem, Outcome<T>> rescue)
            => (await self).Rescue(rescue);

        /// <summary>
        /// Async sad-path composition operator.
        /// If the async outcome resolves to a problem, the rescue function is executed to attempt to recover from the problem and produce the next outcome.
        /// If the async outcome resolves to a value , the rescue function is not executed and the current outcome returned.
        /// </summary>
        /// <param name="rescue">Function to execute to attempt recovery from the problem.</param>
        /// <returns>The current async outcome if it resolves to a value, otherwise a new async outcome representing the result of the rescue function.</returns>
        public AsyncOutcome<T> Rescue(Func<Problem, Outcome<T>> rescue)
            => self.With(self.Local(rescue));
    }
}