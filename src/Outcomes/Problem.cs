namespace WarpCode.Outcomes;

/// <summary>
/// Represents a simple, immutable data object intended to replace the practice of
/// throwing exceptions in your code when a business rule fails.
/// </summary>
/// <param name="Detail">Human-readable detail of the problem that occured.</param>
public record Problem(string Detail)
{
    /// <summary>
    /// Human-readable detail of the problem that occured.
    /// </summary>
    public string Detail { get; init; } = Detail ?? throw new ArgumentNullException(nameof(Detail));
}
