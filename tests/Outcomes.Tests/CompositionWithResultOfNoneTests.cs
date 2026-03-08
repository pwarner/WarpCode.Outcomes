namespace WarpCode.Outcomes.Tests;

public class CompositionWithResultOfNoneTests : CompositionTestBase
{
    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public void ShouldComposeResultAndResult(ProblemStep step)
    {
        Result<string> composition =
            StringResult(step)
                | (_ => EmptyResult(step));

        AssertExpectedResult(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ShouldComposeResultAndResultTask(ProblemStep step)
    {
        ValueTask<Result<string>> composition =
            StringResult(step)
                | (_ => EmptyResultTask(step));

        AssertExpectedResult(step, await composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ShouldComposeResultAndResultValueTask(ProblemStep step)
    {
        ValueTask<Result<string>> composition =
            StringResult(step)
                | (_ => EmptyResultValueTask(step));

        AssertExpectedResult(step, await composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ShouldComposeResultTaskAndResult(ProblemStep step)
    {
        ValueTask<Result<string>> composition =
            StringResultTask(step)
                | (_ => EmptyResult(step));

        AssertExpectedResult(step, await composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ShouldComposeResultTaskAndResultTask(ProblemStep step)
    {
        ValueTask<Result<string>> composition =
            StringResultTask(step)
                | (_ => EmptyResultTask(step));

        AssertExpectedResult(step, await composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ShouldComposeResultTaskAndResultValueTask(ProblemStep step)
    {
        ValueTask<Result<string>> composition =
            StringResultTask(step)
                | (_ => EmptyResultValueTask(step));

        AssertExpectedResult(step, await composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ShouldComposeResultValueTaskAndResult(ProblemStep step)
    {
        ValueTask<Result<string>> composition =
            StringResultValueTask(step)
                | (_ => EmptyResult(step));

        AssertExpectedResult(step, await composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ShouldComposeResultValueTaskAndResultTask(ProblemStep step)
    {
        ValueTask<Result<string>> composition =
            StringResultValueTask(step)
                | (_ => EmptyResultTask(step));

        AssertExpectedResult(step, await composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public async Task ShouldComposeResultValueTaskAndResultValueTask(ProblemStep step)
    {
        ValueTask<Result<string>> composition =
            StringResultValueTask(step)
                | (_ => EmptyResultValueTask(step));

        AssertExpectedResult(step, await composition);
    }
}
