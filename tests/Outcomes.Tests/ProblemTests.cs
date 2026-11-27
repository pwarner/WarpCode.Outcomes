namespace WarpCode.Outcomes.Tests;

public class ProblemTests
{
    [Fact]
    public void Constructor_ShouldThrow_WhenDetailIsNull()
        => Assert.Equal("Detail", Assert.Throws<ArgumentNullException>(() => new Problem(null!)).ParamName);
}
