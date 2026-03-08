namespace WarpCode.Outcomes.Tests;

public abstract class CompositionTestBase
{
    protected const string Success = "success";
    protected static readonly Problem TestProblem1 = new("ruh roh!");
    protected static readonly Problem TestProblem2 = new("dammit!");

    protected static void AssertExpectedResult(ProblemStep step, Result<string> composition)
    {
        var actual = composition.Match(value => value, p => p.Detail);
        var expected = step switch
        {
            ProblemStep.First => TestProblem1.Detail,
            ProblemStep.Second => TestProblem2.Detail,
            _ => Success
        };

        Assert.Equal(expected, actual);
    }

    protected static Task<Result<None>> EmptyResultTask(ProblemStep step) =>
        Task.FromResult(EmptyResult(step));

    protected static ValueTask<Result<None>> EmptyResultValueTask(ProblemStep step) =>
        ValueTask.FromResult(EmptyResult(step));

    protected static Result<None> EmptyResult(ProblemStep step) =>
        step is ProblemStep.First ? TestProblem1 : Result.Ok;

    protected static Task<Result<string>> StringResultTask(ProblemStep step) =>
        Task.FromResult(StringResult(step));

    protected static ValueTask<Result<string>> StringResultValueTask(ProblemStep step) =>
        ValueTask.FromResult(StringResult(step));

    protected static Result<string> StringResult(ProblemStep step) =>
        step is ProblemStep.Second ? TestProblem2 : Success;
}
