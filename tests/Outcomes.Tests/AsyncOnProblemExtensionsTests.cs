namespace WarpCode.Outcomes.Tests;

public class AsyncOnProblemExtensionsTests
{
    private static readonly Problem TestProblem = new(nameof(TestProblem));

    [Fact]
    public async Task OnProblem_ShouldInvokeWithProblemAndReturnOriginal_WhenProblem()
    {
        Problem? seen = null;

        var actual = await Async.OfProblem<int>(TestProblem).OnProblem(p => seen = p);

        Assert.Equal(Outcome.OfProblem<int>(TestProblem), actual);
        Assert.Equal(TestProblem, seen);
    }

    [Fact]
    public async Task OnProblem_ShouldNotInvokeAndReturnOriginal_WhenSuccess()
    {
        var invoked = false;

        var actual = await Async.Of(10).OnProblem(_ => invoked = true);

        Assert.Equal(Outcome.Of(10), actual);
        Assert.False(invoked);
    }
}
