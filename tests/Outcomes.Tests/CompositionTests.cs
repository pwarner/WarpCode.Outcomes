namespace WarpCode.Outcomes.Tests;

public class CompositionTests : CompositionTestBase
{
    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.None)]
    public void ShouldMapResult(ProblemStep step)
    {
        Result<string> composition =
            StringResult(step)
                | (_ => Success);

        AssertExpectedResult(step, composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.None)]
    public async Task ShouldMapResultTask(ProblemStep step)
    {
        ValueTask<Result<string>> composition =
            StringResultTask(step)
                | (_ => Success);

        AssertExpectedResult(step, await composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.None)]
    public async Task ShouldMapResultValueTask(ProblemStep step)
    {
        ValueTask<Result<string>> composition =
            StringResultValueTask(step)
                | (_ => Success);

        AssertExpectedResult(step, await composition);
    }

    [Theory]
    [InlineData(ProblemStep.First)]
    [InlineData(ProblemStep.Second)]
    [InlineData(ProblemStep.None)]
    public void ShouldComposeResultAndResult(ProblemStep step)
    {
        Result<string> composition =
            StringResult(step)
                | (_ => StringResult(step));

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
                | (_ => StringResultTask(step));

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
                | (_ => StringResultValueTask(step));

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
                | (_ => StringResult(step));

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
                | (_ => StringResultTask(step));

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
                | (_ => StringResultValueTask(step));

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
                | (_ => StringResult(step));

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
                | (_ => StringResultTask(step));

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
                | (_ => StringResultValueTask(step));

        AssertExpectedResult(step, await composition);
    }
}
