namespace WarpCode.Outcomes.Tests;

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

    public AdaptationTests() =>
        // only use global mapping with explicit global mapping tests
        Adapt.MapExceptions = null;

    [Fact]
    public void Adapt_From_Func_ShouldCreateSuccessResultIfNoErrorThrown()
    {
        static int Func() => 13;

        var actual = Adapt.ToResult(Func, TestMap);

        Assert.Equal(Result.Of(13), actual);
    }

    [Fact]
    public void Adapt_From_Func_ShouldCreateProblemResultIfErrorMapped()
    {
        var actual = ThrowFunc.ToResult(TestMap);

        Assert.Equal(Result.OfProblem<int>(Message), actual);
    }

    [Fact]
    public void Adapt_From_Func_ShouldCreateProblemResultIfErrorMapped_Strongly()
    {
        var actual = ThrowFunc.ToResult<int, ApplicationException>(StrongTestMap);

        Assert.Equal(Result.OfProblem<int>(Message), actual);
    }

    [Fact]
    public void Adapt_From_Func_ShouldCreateProblemResultIfErrorMappedGlobally()
    {
        Adapt.MapExceptions = TestMap;

        var actual = ThrowFunc.ToResult();

        Assert.Equal(Result.OfProblem<int>(Message), actual);
    }

    [Fact]
    public void Adapt_From_Func_ShouldThrowIfErrorNotMapped()
    {
        ApplicationException error = Assert.Throws<ApplicationException>(() => ThrowFunc.ToResult());

        Assert.Same(Message, error.Message);
    }

    [Fact]
    public void Adapt_From_Action_ShouldCreateSucessResultIfNoErrorThrown()
    {
        static void Action()
        {
        }

        var actual = Adapt.ToResult(Action, TestMap);

        Assert.Equal(Result.Ok, actual);
    }

    [Fact]
    public void Adapt_From_Action_ShouldCreateProblemResultIfErrorMapped()
    {
        var actual = ThrowAction.ToResult(TestMap);

        Assert.Equal(Result.OfProblem(Message), actual);
    }

    [Fact]
    public void Adapt_From_Action_ShouldCreateProblemResultIfErrorMapped_Strongly()
    {
        var actual = ThrowAction.ToResult<ApplicationException>(StrongTestMap);

        Assert.Equal(Result.OfProblem(Message), actual);
    }

    [Fact]
    public void Adapt_From_Action_ShouldCreateProblemResultIfErrorMappedGlobally()
    {
        Adapt.MapExceptions = TestMap;

        var actual = ThrowAction.ToResult();

        Assert.Equal(Result.OfProblem(Message), actual);
    }

    [Fact]
    public void Adapt_From_Action_ShouldThrowIfErrorNotMapped()
    {
        ApplicationException error = Assert.Throws<ApplicationException>(() => ThrowAction.ToResult());

        Assert.Same(Message, error.Message);
    }

    [Fact]
    public async Task Adapt_From_ValueTaskOfT_ShouldCreateSuccessResultIfNoErrorThrown()
    {
        ValueTask<int> valueTask = new(13);

        Result<int> actual = await valueTask.ToResult();

        Assert.Equal(Result.Of(13), actual);
    }

    [Fact]
    public async Task Adapt_From_ValueTaskOfT_ShouldCreateProblemResultIfErrorMapped()
    {
        ValueTask<int> valueTask = ValueTask.FromException<int>(new ApplicationException(Message));

        Result<int> actual = await valueTask.ToResult(TestMap);

        Assert.Equal(Result.OfProblem<int>(Message), actual);
    }

    [Fact]
    public async Task Adapt_From_ValueTaskOfT_ShouldCreateProblemResultIfErrorMapped_Strongly()
    {
        ValueTask<int> valueTask = ValueTask.FromException<int>(new ApplicationException(Message));

        Result<int> actual = await valueTask.ToResult<int, ApplicationException>(StrongTestMap);

        Assert.Equal(Result.OfProblem<int>(Message), actual);
    }

    [Fact]
    public async Task Adapt_From_ValueTaskOfT_ShouldCreateProblemResultIfErrorMappedGlobally()
    {
        ValueTask<int> valueTask = ValueTask.FromException<int>(new ApplicationException(Message));

        Adapt.MapExceptions = TestMap;

        Result<int> actual = await valueTask.ToResult();

        Assert.Equal(Result.OfProblem<int>(Message), actual);
    }

    [Fact]
    public async Task Adapt_From_ValueTaskOfT_ShouldThrowIfErrorNotMapped()
    {
        ValueTask<int> valueTask = ValueTask.FromException<int>(new ApplicationException(Message));

        ApplicationException error = await Assert.ThrowsAsync<ApplicationException>(async () =>
            await valueTask.ToResult());

        Assert.Same(Message, error.Message);
    }

    [Fact]
    public async Task Adapt_From_TaskOfT_ShouldCreateSuccessResultIfNoErrorThrown()
    {
        Task<int> task = Task.FromResult(13);

        Result<int> actual = await task.ToResult();

        Assert.Equal(Result.Of(13), actual);
    }

    [Fact]
    public async Task Adapt_From_TaskOfT_ShouldCreateProblemResultIfErrorMapped()
    {
        Task<int> task = Task.FromException<int>(new ApplicationException(Message));

        Result<int> actual = await task.ToResult(TestMap);

        Assert.Equal(Result.OfProblem<int>(Message), actual);
    }

    [Fact]
    public async Task Adapt_From_TaskOfT_ShouldCreateProblemResultIfErrorMapped_Strongly()
    {
        Task<int> task = Task.FromException<int>(new ApplicationException(Message));

        Result<int> actual = await task.ToResult<int, ApplicationException>(StrongTestMap);

        Assert.Equal(Result.OfProblem<int>(Message), actual);
    }

    [Fact]
    public async Task Adapt_From_TaskOfT_ShouldCreateProblemResultIfErrorMappedGlobally()
    {
        Task<int> task = Task.FromException<int>(new ApplicationException(Message));

        Adapt.MapExceptions = TestMap;

        Result<int> actual = await task.ToResult();

        Assert.Equal(Result.OfProblem<int>(Message), actual);
    }

    [Fact]
    public async Task Adapt_From_TaskOfT_ShouldThrowIfErrorNotMapped()
    {
        Task<int> task = Task.FromException<int>(new ApplicationException(Message));

        ApplicationException error = await Assert.ThrowsAsync<ApplicationException>(async () =>
            await task.ToResult());

        Assert.Same(Message, error.Message);
    }

    [Fact]
    public async Task Adapt_From_ValueTask_ShouldCreateSuccessResultIfNoErrorThrown()
    {
        ValueTask valueTask = ValueTask.CompletedTask;

        Result<None> actual = await valueTask.ToResult();

        Assert.Equal(Result.Ok, actual);
    }

    [Fact]
    public async Task Adapt_From_ValueTask_ShouldCreateProblemResultIfErrorMapped()
    {
        var valueTask = ValueTask.FromException(new ApplicationException(Message));

        Result<None> actual = await valueTask.ToResult(TestMap);

        Assert.Equal(Result.OfProblem(Message), actual);
    }

    [Fact]
    public async Task Adapt_From_ValueTask_ShouldCreateProblemResultIfErrorMapped_Strongly()
    {
        var valueTask = ValueTask.FromException(new ApplicationException(Message));

        Result<None> actual = await valueTask.ToResult<ApplicationException>(StrongTestMap);

        Assert.Equal(Result.OfProblem(Message), actual);
    }

    [Fact]
    public async Task Adapt_From_ValueTask_ShouldCreateProblemResultIfErrorMappedGlobally()
    {
        var valueTask = ValueTask.FromException(new ApplicationException(Message));

        Adapt.MapExceptions = TestMap;

        Result<None> actual = await valueTask.ToResult();

        Assert.Equal(Result.OfProblem(Message), actual);
    }

    [Fact]
    public async Task Adapt_From_ValueTask_ShouldThrowIfErrorNotMapped()
    {
        var valueTask = ValueTask.FromException(new ApplicationException(Message));

        ApplicationException error = await Assert.ThrowsAsync<ApplicationException>(async () =>
            await valueTask.ToResult());

        Assert.Same(Message, error.Message);
    }

    [Fact]
    public async Task Adapt_From_Task_ShouldCreateSuccessResultIfNoErrorThrown()
    {
        Task task = Task.CompletedTask;

        Result<None> actual = await task.ToResult();

        Assert.Equal(Result.Ok, actual);
    }

    [Fact]
    public async Task Adapt_From_Task_ShouldCreateProblemResultIfErrorMapped()
    {
        var task = Task.FromException(new ApplicationException(Message));

        Result<None> actual = await task.ToResult(TestMap);

        Assert.Equal(Result.OfProblem(Message), actual);
    }

    [Fact]
    public async Task Adapt_From_Task_ShouldCreateProblemResultIfErrorMapped_Strongly()
    {
        var task = Task.FromException(new ApplicationException(Message));

        Result<None> actual = await task.ToResult<ApplicationException>(StrongTestMap);

        Assert.Equal(Result.OfProblem(Message), actual);
    }

    [Fact]
    public async Task Adapt_From_Task_ShouldCreateProblemResultIfErrorMappedGlobally()
    {
        var task = Task.FromException(new ApplicationException(Message));

        Adapt.MapExceptions = TestMap;

        Result<None> actual = await task.ToResult();

        Assert.Equal(Result.OfProblem(Message), actual);
    }

    [Fact]
    public async Task Adapt_From_Task_ShouldThrowIfErrorNotMapped()
    {
        var task = Task.FromException(new ApplicationException(Message));

        ApplicationException error = await Assert.ThrowsAsync<ApplicationException>(async () =>
            await task.ToResult());

        Assert.Same(Message, error.Message);
    }
}
