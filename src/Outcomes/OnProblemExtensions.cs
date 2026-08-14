namespace WarpCode.Outcomes;

/// <summary>
/// `OnProblem` composition extensions
/// </summary>
public static class OnProblemExtensions
{
    /// <param name="self">Current outcome.</param>
    extension<T>(Outcome<T> self)
    {

        /// <summary>
        /// Sad-path composition operator.
        /// If the outcome contains a problem, the onProblem action is executed.
        /// If the outcome contains a value, the onProblem action is not executed and the current outcome is returned.
        /// </summary>
        /// <param name="onProblem">Action to execute if the outcome is a problem.</param>
        /// <returns>The current outcome.</returns>
        public Outcome<T> OnProblem(Action<Problem> onProblem)
            => self.Rescue(problem =>
            {
                onProblem(problem);
                return self;
            });
    }
}