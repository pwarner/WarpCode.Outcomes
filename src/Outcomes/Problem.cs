namespace WarpCode.Outcomes;

/// <summary>
/// Default implementation of <see cref="IProblem"/>.
/// </summary>
/// <remarks>
/// Creates a new problem with the provided detail.
/// </remarks>
/// <param name="detail"><see cref="Detail"/> parameter.</param>
public record Problem(string detail) : IProblem
{
    /// <inheritdoc />
    public string Detail { get; init; } = detail ?? throw new ArgumentNullException(nameof(detail));
}
