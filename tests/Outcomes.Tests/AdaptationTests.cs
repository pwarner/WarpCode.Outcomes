namespace Outcomes.Tests;

public class AdaptationTests
{
    private const string Message = "Something went wrong";

    private static Problem? MapExceptions(Exception e) =>
        e switch
        {
            ApplicationException ex => new Problem(ex.Message),
            _ => null
        };

    [Fact]
    public void Adapt_From_Func_ShouldCreateSucessOutcomeIfNoErrorThrown()
    {
        static int Func() => 13;

        Outcome<int> actual = Adapt.ToOutcome(Func, MapExceptions);

        Assert.Equal(new Outcome<int>(13), actual);
    }

    [Fact]
    public void Adapt_From_Func_ShouldCreateProblemOutcomeIfErrorMapped()
    {
        static int Func() => throw new ApplicationException(Message);

        Outcome<int> actual = Adapt.ToOutcome(Func, MapExceptions);

        Assert.Equal(new Outcome<int>(new Problem(Message)), actual);
    }

    [Fact]
    public void Adapt_From_Func_ShouldThrowIfErrorNotMapped()
    {
        static int Func() => throw new ApplicationException(Message);

        var error = Assert.Throws<ApplicationException>(() => Adapt.ToOutcome(Func));

        Assert.Same(Message, error.Message);
    }

    [Fact]
    public void Adapt_From_Action_ShouldCreateSucessOutcomeIfNoErrorThrown()
    {
        static void Action()
        {
        }

        Outcome<None> actual = Adapt.ToOutcome(Action, MapExceptions);

        Assert.Equal(new Outcome<None>(), actual);
    }

    [Fact]
    public void Adapt_From_Action_ShouldCreateProblemOutcomeIfErrorMapped()
    {
        const string message = "Something went wrong";

        static void Action() => throw new ApplicationException(message);

        Outcome<None> actual = Adapt.ToOutcome(Action, MapExceptions);

        Assert.Equal(new Outcome<None>(new Problem(message)), actual);
    }

    [Fact]
    public void Adapt_From_Action_ShouldThrowIfErrorNotMapped()
    {
        static void Action() => throw new ApplicationException(Message);

        var error = Assert.Throws<ApplicationException>(() => Adapt.ToOutcome(Action));

        Assert.Same(Message, error.Message);
    }
}
