namespace Outcomes.Tests;

public class AdaptationTests
{
    private const string Message = "Something went wrong";

    private static Problem? TestMap(Exception e) =>
        e switch
        {
            ApplicationException ex => new Problem(ex.Message),
            _ => null
        };

    private static Problem StrongTestMap(ApplicationException e) => new(e.Message);

    private static readonly Func<int> ThrowFunc = () => throw new ApplicationException(Message);

    private static readonly Action ThrowAction = () => throw new ApplicationException(Message);

    public AdaptationTests()
    {
        // only use global mapping with explicit global mapping tests
        Adapt.MapExceptions = null;
    }

    [Fact]
    public void Adapt_From_Func_ShouldCreateSucessOutcomeIfNoErrorThrown()
    {
        static int Func() => 13;

        Outcome<int> actual = Adapt.ToOutcome(Func, TestMap);

        Assert.Equal(Outcome.Of(13), actual);
    }

    [Fact]
    public void Adapt_From_Func_ShouldCreateProblemOutcomeIfErrorMapped()
    {
        Outcome<int> actual = ThrowFunc.ToOutcome(TestMap);

        Assert.Equal(new Problem(Message).ToOutcome<int>(), actual);
    }

    [Fact]
    public void Adapt_From_Func_ShouldCreateProblemOutcomeIfErrorMapped_Strongly()
    {
        Outcome<int> actual = ThrowFunc.ToOutcome<int, ApplicationException>(StrongTestMap);

        Assert.Equal(new Problem(Message).ToOutcome<int>(), actual);
    }

    [Fact]
    public void Adapt_From_Func_ShouldCreateProblemOutcomeIfErrorMappedGlobally()
    {
        Adapt.MapExceptions = TestMap;

        Outcome<int> actual = ThrowFunc.ToOutcome();

        Assert.Equal(new Problem(Message).ToOutcome<int>(), actual);
    }

    [Fact]
    public void Adapt_From_Func_ShouldThrowIfErrorNotMapped()
    {
        var error = Assert.Throws<ApplicationException>(() => ThrowFunc.ToOutcome());

        Assert.Same(Message, error.Message);
    }

    [Fact]
    public void Adapt_From_Action_ShouldCreateSucessOutcomeIfNoErrorThrown()
    {
        static void Action()
        {
        }

        Outcome<None> actual = Adapt.ToOutcome(Action, TestMap);

        Assert.Equal(Outcome.Ok, actual);
    }

    [Fact]
    public void Adapt_From_Action_ShouldCreateProblemOutcomeIfErrorMapped()
    {
        Outcome<None> actual = ThrowAction.ToOutcome(TestMap);

        Assert.Equal(new Problem(Message).ToOutcome(), actual);
    }

    [Fact]
    public void Adapt_From_Action_ShouldCreateProblemOutcomeIfErrorMapped_Strongly()
    {
        Outcome<None> actual = ThrowAction.ToOutcome<ApplicationException>(StrongTestMap);

        Assert.Equal(new Problem(Message).ToOutcome(), actual);
    }

    [Fact]
    public void Adapt_From_Action_ShouldCreateProblemOutcomeIfErrorMappedGlobally()
    {
        Adapt.MapExceptions = TestMap;

        Outcome<None> actual = ThrowAction.ToOutcome();

        Assert.Equal(new Problem(Message).ToOutcome(), actual);
    }

    [Fact]
    public void Adapt_From_Action_ShouldThrowIfErrorNotMapped()
    {
        var error = Assert.Throws<ApplicationException>(() => ThrowAction.ToOutcome());

        Assert.Same(Message, error.Message);
    }

    [Fact]
    public async Task Adapt_From_ValueTaskOfT_ShouldCreateSuccessOutcomeIfNoErrorThrown()
    {
        ValueTask<int> valueTask = new(13);

        Outcome<int> actual = await valueTask.ToOutcome();

        Assert.Equal(Outcome.Of(13), actual);
    }

    [Fact]
    public async Task Adapt_From_ValueTaskOfT_ShouldCreateProblemOutcomeIfErrorMapped()
    {
        ValueTask<int> valueTask = ValueTask.FromException<int>(new ApplicationException(Message));

        Outcome<int> actual = await valueTask.ToOutcome(TestMap);

        Assert.Equal(new Problem(Message).ToOutcome<int>(), actual);
    }

    [Fact]
    public async Task Adapt_From_ValueTaskOfT_ShouldCreateProblemOutcomeIfErrorMapped_Strongly()
    {
        ValueTask<int> valueTask = ValueTask.FromException<int>(new ApplicationException(Message));

        Outcome<int> actual = await valueTask.ToOutcome<int, ApplicationException>(StrongTestMap);

        Assert.Equal(new Problem(Message).ToOutcome<int>(), actual);
    }

    [Fact]
    public async Task Adapt_From_ValueTaskOfT_ShouldCreateProblemOutcomeIfErrorMappedGlobally()
    {
        ValueTask<int> valueTask = ValueTask.FromException<int>(new ApplicationException(Message));

        Adapt.MapExceptions = TestMap;

        Outcome<int> actual = await valueTask.ToOutcome();

        Assert.Equal(new Problem(Message).ToOutcome<int>(), actual);
    }

    [Fact]
    public async Task Adapt_From_ValueTaskOfT_ShouldThrowIfErrorNotMapped()
    {
        ValueTask<int> valueTask = ValueTask.FromException<int>(new ApplicationException(Message));

        var error = await Assert.ThrowsAsync<ApplicationException>(async () =>
            await valueTask.ToOutcome());

        Assert.Same(Message, error.Message);
    }

    [Fact]
    public async Task Adapt_From_TaskOfT_ShouldCreateSuccessOutcomeIfNoErrorThrown()
    {
        Task<int> task = Task.FromResult(13);

        Outcome<int> actual = await task.ToOutcome();

        Assert.Equal(Outcome.Of(13), actual);
    }

    [Fact]
    public async Task Adapt_From_TaskOfT_ShouldCreateProblemOutcomeIfErrorMapped()
    {
        Task<int> task = Task.FromException<int>(new ApplicationException(Message));

        Outcome<int> actual = await task.ToOutcome(TestMap);

        Assert.Equal(new Problem(Message).ToOutcome<int>(), actual);
    }

    [Fact]
    public async Task Adapt_From_TaskOfT_ShouldCreateProblemOutcomeIfErrorMapped_Strongly()
    {
        Task<int> task = Task.FromException<int>(new ApplicationException(Message));

        Outcome<int> actual = await task.ToOutcome<int, ApplicationException>(StrongTestMap);

        Assert.Equal(new Problem(Message).ToOutcome<int>(), actual);
    }

    [Fact]
    public async Task Adapt_From_TaskOfT_ShouldCreateProblemOutcomeIfErrorMappedGlobally()
    {
        Task<int> task = Task.FromException<int>(new ApplicationException(Message));

        Adapt.MapExceptions = TestMap;

        Outcome<int> actual = await task.ToOutcome();

        Assert.Equal(new Problem(Message).ToOutcome<int>(), actual);
    }

    [Fact]
    public async Task Adapt_From_TaskOfT_ShouldThrowIfErrorNotMapped()
    {
        Task<int> task = Task.FromException<int>(new ApplicationException(Message));

        var error = await Assert.ThrowsAsync<ApplicationException>(async () =>
            await task.ToOutcome());

        Assert.Same(Message, error.Message);
    }

    [Fact]
    public async Task Adapt_From_ValueTask_ShouldCreateSuccessOutcomeIfNoErrorThrown()
    {
        ValueTask valueTask = ValueTask.CompletedTask;

        Outcome<None> actual = await valueTask.ToOutcome();

        Assert.Equal(Outcome.Ok, actual);
    }

    [Fact]
    public async Task Adapt_From_ValueTask_ShouldCreateProblemOutcomeIfErrorMapped()
    {
        ValueTask valueTask = ValueTask.FromException(new ApplicationException(Message));

        Outcome<None> actual = await valueTask.ToOutcome(TestMap);

        Assert.Equal(new Problem(Message).ToOutcome(), actual);
    }

    [Fact]
    public async Task Adapt_From_ValueTask_ShouldCreateProblemOutcomeIfErrorMapped_Strongly()
    {
        ValueTask valueTask = ValueTask.FromException(new ApplicationException(Message));

        Outcome<None> actual = await valueTask.ToOutcome<ApplicationException>(StrongTestMap);

        Assert.Equal(new Problem(Message).ToOutcome(), actual);
    }

    [Fact]
    public async Task Adapt_From_ValueTask_ShouldCreateProblemOutcomeIfErrorMappedGlobally()
    {
        ValueTask valueTask = ValueTask.FromException(new ApplicationException(Message));

        Adapt.MapExceptions = TestMap;

        Outcome<None> actual = await valueTask.ToOutcome();

        Assert.Equal(new Problem(Message).ToOutcome(), actual);
    }

    [Fact]
    public async Task Adapt_From_ValueTask_ShouldThrowIfErrorNotMapped()
    {
        ValueTask valueTask = ValueTask.FromException(new ApplicationException(Message));

        var error = await Assert.ThrowsAsync<ApplicationException>(async () =>
            await valueTask.ToOutcome());

        Assert.Same(Message, error.Message);
    }

    [Fact]
    public async Task Adapt_From_Task_ShouldCreateSuccessOutcomeIfNoErrorThrown()
    {
        Task task = Task.CompletedTask;

        Outcome<None> actual = await task.ToOutcome();

        Assert.Equal(Outcome.Ok, actual);
    }

    [Fact]
    public async Task Adapt_From_Task_ShouldCreateProblemOutcomeIfErrorMapped()
    {
        Task task = Task.FromException(new ApplicationException(Message));

        Outcome<None> actual = await task.ToOutcome(TestMap);

        Assert.Equal(new Problem(Message).ToOutcome(), actual);
    }

    [Fact]
    public async Task Adapt_From_Task_ShouldCreateProblemOutcomeIfErrorMapped_Strongly()
    {
        Task task = Task.FromException(new ApplicationException(Message));

        Outcome<None> actual = await task.ToOutcome<ApplicationException>(StrongTestMap);

        Assert.Equal(new Problem(Message).ToOutcome(), actual);
    }

    [Fact]
    public async Task Adapt_From_Task_ShouldCreateProblemOutcomeIfErrorMappedGlobally()
    {
        Task task = Task.FromException(new ApplicationException(Message));

        Adapt.MapExceptions = TestMap;

        Outcome<None> actual = await task.ToOutcome();

        Assert.Equal(new Problem(Message).ToOutcome(), actual);
    }

    [Fact]
    public async Task Adapt_From_Task_ShouldThrowIfErrorNotMapped()
    {
        Task task = Task.FromException(new ApplicationException(Message));

        var error = await Assert.ThrowsAsync<ApplicationException>(async () =>
            await task.ToOutcome());

        Assert.Same(Message, error.Message);
    }
}
