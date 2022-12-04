namespace Outcomes.Tests;

public class ProblemResolutionTests
{
    [Theory]
    [MemberData(nameof(ProblemData), 4)]
    public void CanResolve4StronglyTypedProblems(IProblem problem, string expected)
    {
        string? actual = problem.ToOutcome()
            .Resolve(
                _ => null!,
                p => p.GetType().Name,
                (Problem1 p1) => p1.GetType().Name,
                (Problem2 p2) => p2.GetType().Name,
                (Problem3 p3) => p3.GetType().Name,
                (Problem4 p4) => p4.GetType().Name
            );

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ProblemData), 3)]
    public void CanResolve3StronglyTypedProblems(IProblem problem, string expected)
    {
        string? actual = problem.ToOutcome()
            .Resolve(
                _ => null!,
                p => p.GetType().Name,
                (Problem1 p1) => p1.GetType().Name,
                (Problem2 p2) => p2.GetType().Name,
                (Problem3 p3) => p3.GetType().Name
            );

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ProblemData), 2)]
    public void CanResolve2StronglyTypedProblems(IProblem problem, string expected)
    {
        string? actual = problem.ToOutcome()
            .Resolve(
                _ => null!,
                p => p.GetType().Name,
                (Problem1 p1) => p1.GetType().Name,
                (Problem2 p2) => p2.GetType().Name
            );

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ProblemData), 1)]
    public void CanResolve1StronglyTypedProblems(IProblem problem, string expected)
    {
        string? actual = problem.ToOutcome()
            .Resolve(
                _ => null!,
                p => p.GetType().Name,
                (Problem1 p1) => p1.GetType().Name
            );

        Assert.Equal(expected, actual);
    }

    public static IEnumerable<object[]> ProblemData(int take) =>
        new[]
        {
            new object[] { new Problem("Some other problem"), nameof(Problem) },
            new object[] { Problem1.Instance, nameof(Problem1) },
            new object[] { Problem2.Instance, nameof(Problem2) },
            new object[] { Problem3.Instance, nameof(Problem3) },
            new object[] { Problem4.Instance, nameof(Problem4) }
        }.Take(take + 1);
}

internal sealed class Problem1 : Problem
{
    public static readonly Problem1 Instance = new();

    public Problem1() : base(nameof(Problem1))
    {
    }
}

internal sealed class Problem2 : Problem
{
    public static readonly Problem2 Instance = new();

    public Problem2() : base(nameof(Problem1))
    {
    }
}

internal sealed class Problem3 : Problem
{
    public static readonly Problem3 Instance = new();

    public Problem3() : base(nameof(Problem1))
    {
    }
}

internal sealed class Problem4 : Problem
{
    public static readonly Problem4 Instance = new();

    public Problem4() : base(nameof(Problem1))
    {
    }
}
