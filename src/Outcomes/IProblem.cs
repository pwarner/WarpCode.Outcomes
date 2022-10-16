namespace Outcomes;

/// <summary>
/// Represents a simple, immutable data object intended to replace the practice of
/// throwing exceptions in your code when a business rule fails.
/// </summary>
public interface IProblem
{
    /// <summary>
    /// Human-readable detail of the problem that occured.
    /// </summary>
    string Detail { get; }
}
