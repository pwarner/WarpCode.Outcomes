namespace WarpCode.Outcomes;

/// <summary>
/// `Rescue` composition extensions
/// </summary>
public static class RescueExtensions
{
    /// <param name="self">Current outcome.</param>
    extension<T>(Outcome<T> self)
    {
        /// <summary>
        /// Sad-path composition operator.
        /// If the outcome contains a problem, the rescue function is executed to attempt to recover from the problem and produce the next outcome.
        /// If the outcome contains a value , the rescue function is not executed and the current outcome returned.
        /// </summary>
        /// <param name="rescue">Function to execute to attempt recovery from the problem.</param>
        /// <returns>The current outcome if it resolves to a value, otherwise a new outcome representing the result of the rescue function.</returns>
        public Outcome<T> Rescue(Func<Problem, Outcome<T>> rescue)
            => self.Match(_ => self, rescue);
    }
}