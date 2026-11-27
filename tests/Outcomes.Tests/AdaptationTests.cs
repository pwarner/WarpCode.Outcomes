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
    public void Adapt_WithTypedMap_ShouldThrow_WhenMapIsNull()
        => Assert.Equal("map", Assert.Throws<ArgumentNullException>(() => ThrowFunc.ToOutcome<int, ApplicationException>(null!)).ParamName);

    [Fact]
    public void Adapt_From_Func_ShouldCreateSuccessOutcomeIfNoErrorThrown()
    {
        static int Func() => 13;

        var actual = Adapt.ToOutcome(Func, TestMap);

        Assert.Equal(Outcome.Of(13), actual);
    }

    [Fact]
    public void Adapt_From_Func_ShouldCreateProblemOutcomeIfErrorMapped()
    {
        var actual = ThrowFunc.ToOutcome(TestMap);

        Assert.Equal(Outcome.OfProblem<int>(Message), actual);
    }

    [Fact]
    public void Adapt_From_Func_ShouldCreateProblemOutcomeIfErrorMapped_Strongly()
    {
        var actual = ThrowFunc.ToOutcome<int, ApplicationException>(StrongTestMap);

        Assert.Equal(Outcome.OfProblem<int>(Message), actual);
    }

    [Fact]
    public void Adapt_From_Func_ShouldCreateProblemOutcomeIfErrorMappedGlobally()
    {
        Adapt.MapExceptions = TestMap;

        var actual = ThrowFunc.ToOutcome();

        Assert.Equal(Outcome.OfProblem<int>(Message), actual);
    }

    [Fact]
    public void Adapt_From_Func_ShouldThrowIfErrorNotMapped()
    {
        ApplicationException error = Assert.Throws<ApplicationException>(() => ThrowFunc.ToOutcome());

        Assert.Same(Message, error.Message);
    }

    [Fact]
    public void Adapt_From_Action_ShouldCreateSucessOutcomeIfNoErrorThrown()
    {
        static void Action()
        {
        }

        var actual = Adapt.ToOutcome(Action, TestMap);

        Assert.Equal(Outcome.Ok, actual);
    }

    [Fact]
    public void Adapt_From_Action_ShouldCreateProblemOutcomeIfErrorMapped()
    {
        var actual = ThrowAction.ToOutcome(TestMap);

        Assert.Equal(Outcome.OfProblem(Message), actual);
    }

    [Fact]
    public void Adapt_From_Action_ShouldCreateProblemOutcomeIfErrorMapped_Strongly()
    {
        var actual = ThrowAction.ToOutcome<ApplicationException>(StrongTestMap);

        Assert.Equal(Outcome.OfProblem(Message), actual);
    }

    [Fact]
    public void Adapt_From_Action_ShouldCreateProblemOutcomeIfErrorMappedGlobally()
    {
        Adapt.MapExceptions = TestMap;

        var actual = ThrowAction.ToOutcome();

        Assert.Equal(Outcome.OfProblem(Message), actual);
    }

    [Fact]
    public void Adapt_From_Action_ShouldThrowIfErrorNotMapped()
    {
        ApplicationException error = Assert.Throws<ApplicationException>(() => ThrowAction.ToOutcome());

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

        Assert.Equal(Outcome.OfProblem<int>(Message), actual);
    }

    [Fact]
    public async Task Adapt_From_ValueTaskOfT_ShouldCreateProblemOutcomeIfErrorMapped_Strongly()
    {
        ValueTask<int> valueTask = ValueTask.FromException<int>(new ApplicationException(Message));

        Outcome<int> actual = await valueTask.ToOutcome<int, ApplicationException>(StrongTestMap);

        Assert.Equal(Outcome.OfProblem<int>(Message), actual);
    }

    [Fact]
    public async Task Adapt_From_ValueTaskOfT_ShouldCreateProblemOutcomeIfErrorMappedGlobally()
    {
        ValueTask<int> valueTask = ValueTask.FromException<int>(new ApplicationException(Message));

        Adapt.MapExceptions = TestMap;

        Outcome<int> actual = await valueTask.ToOutcome();

        Assert.Equal(Outcome.OfProblem<int>(Message), actual);
    }

    [Fact]
    public async Task Adapt_From_ValueTaskOfT_ShouldThrowIfErrorNotMapped()
    {
        ValueTask<int> valueTask = ValueTask.FromException<int>(new ApplicationException(Message));

        ApplicationException error = await Assert.ThrowsAsync<ApplicationException>(async () =>
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

        Assert.Equal(Outcome.OfProblem<int>(Message), actual);
    }

    [Fact]
    public async Task Adapt_From_TaskOfT_ShouldCreateProblemOutcomeIfErrorMapped_Strongly()
    {
        Task<int> task = Task.FromException<int>(new ApplicationException(Message));

        Outcome<int> actual = await task.ToOutcome<int, ApplicationException>(StrongTestMap);

        Assert.Equal(Outcome.OfProblem<int>(Message), actual);
    }

    [Fact]
    public async Task Adapt_From_TaskOfT_ShouldCreateProblemOutcomeIfErrorMappedGlobally()
    {
        Task<int> task = Task.FromException<int>(new ApplicationException(Message));

        Adapt.MapExceptions = TestMap;

        Outcome<int> actual = await task.ToOutcome();

        Assert.Equal(Outcome.OfProblem<int>(Message), actual);
    }

    [Fact]
    public async Task Adapt_From_TaskOfT_ShouldThrowIfErrorNotMapped()
    {
        Task<int> task = Task.FromException<int>(new ApplicationException(Message));

        ApplicationException error = await Assert.ThrowsAsync<ApplicationException>(async () =>
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
        var valueTask = ValueTask.FromException(new ApplicationException(Message));

        Outcome<None> actual = await valueTask.ToOutcome(TestMap);

        Assert.Equal(Outcome.OfProblem(Message), actual);
    }

    [Fact]
    public async Task Adapt_From_ValueTask_ShouldCreateProblemOutcomeIfErrorMapped_Strongly()
    {
        var valueTask = ValueTask.FromException(new ApplicationException(Message));

        Outcome<None> actual = await valueTask.ToOutcome<ApplicationException>(StrongTestMap);

        Assert.Equal(Outcome.OfProblem(Message), actual);
    }

    [Fact]
    public async Task Adapt_From_ValueTask_ShouldCreateProblemOutcomeIfErrorMappedGlobally()
    {
        var valueTask = ValueTask.FromException(new ApplicationException(Message));

        Adapt.MapExceptions = TestMap;

        Outcome<None> actual = await valueTask.ToOutcome();

        Assert.Equal(Outcome.OfProblem(Message), actual);
    }

    [Fact]
    public async Task Adapt_From_ValueTask_ShouldThrowIfErrorNotMapped()
    {
        var valueTask = ValueTask.FromException(new ApplicationException(Message));

        ApplicationException error = await Assert.ThrowsAsync<ApplicationException>(async () =>
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
        var task = Task.FromException(new ApplicationException(Message));

        Outcome<None> actual = await task.ToOutcome(TestMap);

        Assert.Equal(Outcome.OfProblem(Message), actual);
    }

    [Fact]
    public async Task Adapt_From_Task_ShouldCreateProblemOutcomeIfErrorMapped_Strongly()
    {
        var task = Task.FromException(new ApplicationException(Message));

        Outcome<None> actual = await task.ToOutcome<Task, ApplicationException>(StrongTestMap);

        Assert.Equal(Outcome.OfProblem(Message), actual);
    }

    [Fact]
    public async Task Adapt_From_Task_ShouldCreateProblemOutcomeIfErrorMappedGlobally()
    {
        var task = Task.FromException(new ApplicationException(Message));

        Adapt.MapExceptions = TestMap;

        Outcome<None> actual = await task.ToOutcome();

        Assert.Equal(Outcome.OfProblem(Message), actual);
    }

    [Fact]
    public async Task Adapt_From_Task_ShouldThrowIfErrorNotMapped()
    {
        var task = Task.FromException(new ApplicationException(Message));

        ApplicationException error = await Assert.ThrowsAsync<ApplicationException>(async () =>
            await task.ToOutcome());

        Assert.Same(Message, error.Message);
    }

    // ----- Strongly-typed shortcut, called the recommended way: a typed lambda lets C# infer every type argument -----
    // Each receiver: keeps the value on success, maps the matching exception, and re-throws any other exception.

    private static readonly InvalidOperationException OtherError = new(Message);

    [Fact]
    public void Adapt_From_Func_Strongly_ShouldKeepValue_WhenNoErrorThrown()
        => Assert.Equal(Outcome.Of(13), ((Func<int>)(() => 13)).ToOutcome((ApplicationException e) => StrongTestMap(e)));

    [Fact]
    public void Adapt_From_Func_Strongly_ShouldMapMatchingException()
        => Assert.Equal(Outcome.OfProblem<int>(Message), ThrowFunc.ToOutcome((ApplicationException e) => StrongTestMap(e)));

    [Fact]
    public void Adapt_From_Func_Strongly_ShouldThrow_WhenExceptionTypeDoesNotMatch()
    {
        Func<int> func = () => throw OtherError;

        Assert.Same(OtherError, Assert.Throws<InvalidOperationException>(() => func.ToOutcome((ApplicationException e) => StrongTestMap(e))));
    }

    [Fact]
    public void Adapt_From_Action_Strongly_ShouldBeOk_WhenNoErrorThrown()
        => Assert.Equal(Outcome.Ok, ((Action)(() => { })).ToOutcome((ApplicationException e) => StrongTestMap(e)));

    [Fact]
    public void Adapt_From_Action_Strongly_ShouldMapMatchingException()
        => Assert.Equal(Outcome.OfProblem(Message), ThrowAction.ToOutcome((ApplicationException e) => StrongTestMap(e)));

    [Fact]
    public void Adapt_From_Action_Strongly_ShouldThrow_WhenExceptionTypeDoesNotMatch()
    {
        Action action = () => throw OtherError;

        Assert.Same(OtherError, Assert.Throws<InvalidOperationException>(() => action.ToOutcome((ApplicationException e) => StrongTestMap(e))));
    }

    // The silent-loss regression: this must bind to the Task<T> overload and keep the value, not return Outcome<None>.
    [Fact]
    public async Task Adapt_From_TaskOfT_Strongly_ShouldKeepValue_WhenNoErrorThrown()
    {
        Outcome<int> actual = await Task.FromResult(13).ToOutcome((ApplicationException e) => StrongTestMap(e));

        Assert.Equal(Outcome.Of(13), actual);
    }

    [Fact]
    public async Task Adapt_From_TaskOfT_Strongly_ShouldMapMatchingException()
        => Assert.Equal(Outcome.OfProblem<int>(Message),
            await Task.FromException<int>(new ApplicationException(Message)).ToOutcome((ApplicationException e) => StrongTestMap(e)));

    [Fact]
    public async Task Adapt_From_TaskOfT_Strongly_ShouldThrow_WhenExceptionTypeDoesNotMatch()
        => Assert.Same(OtherError, await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await Task.FromException<int>(OtherError).ToOutcome((ApplicationException e) => StrongTestMap(e))));

    [Fact]
    public async Task Adapt_From_Task_Strongly_ShouldBeOk_WhenNoErrorThrown()
        => Assert.Equal(Outcome.Ok, await Task.CompletedTask.ToOutcome((ApplicationException e) => StrongTestMap(e)));

    [Fact]
    public async Task Adapt_From_Task_Strongly_ShouldMapMatchingException()
        => Assert.Equal(Outcome.OfProblem(Message),
            await Task.FromException(new ApplicationException(Message)).ToOutcome((ApplicationException e) => StrongTestMap(e)));

    [Fact]
    public async Task Adapt_From_Task_Strongly_ShouldThrow_WhenExceptionTypeDoesNotMatch()
        => Assert.Same(OtherError, await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await Task.FromException(OtherError).ToOutcome((ApplicationException e) => StrongTestMap(e))));

    [Fact]
    public async Task Adapt_From_ValueTaskOfT_Strongly_ShouldKeepValue_WhenNoErrorThrown()
        => Assert.Equal(Outcome.Of(13), await ValueTask.FromResult(13).ToOutcome((ApplicationException e) => StrongTestMap(e)));

    [Fact]
    public async Task Adapt_From_ValueTaskOfT_Strongly_ShouldMapMatchingException()
        => Assert.Equal(Outcome.OfProblem<int>(Message),
            await ValueTask.FromException<int>(new ApplicationException(Message)).ToOutcome((ApplicationException e) => StrongTestMap(e)));

    [Fact]
    public async Task Adapt_From_ValueTaskOfT_Strongly_ShouldThrow_WhenExceptionTypeDoesNotMatch()
        => Assert.Same(OtherError, await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await ValueTask.FromException<int>(OtherError).ToOutcome((ApplicationException e) => StrongTestMap(e))));

    [Fact]
    public async Task Adapt_From_ValueTask_Strongly_ShouldBeOk_WhenNoErrorThrown()
        => Assert.Equal(Outcome.Ok, await ValueTask.CompletedTask.ToOutcome((ApplicationException e) => StrongTestMap(e)));

    [Fact]
    public async Task Adapt_From_ValueTask_Strongly_ShouldMapMatchingException()
        => Assert.Equal(Outcome.OfProblem(Message),
            await ValueTask.FromException(new ApplicationException(Message)).ToOutcome((ApplicationException e) => StrongTestMap(e)));

    [Fact]
    public async Task Adapt_From_ValueTask_Strongly_ShouldThrow_WhenExceptionTypeDoesNotMatch()
        => Assert.Same(OtherError, await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await ValueTask.FromException(OtherError).ToOutcome((ApplicationException e) => StrongTestMap(e))));

    // A Task<T> is also a Task, and explicit type arguments fill an extension block's type parameters first.
    // If the plain-Task strongly-typed shortcut had a plain Task receiver, taskOfT.ToOutcome<SomeException>(map)
    // would compile, bind to it, and silently discard the task's result. Its receiver must stay generic (TTask : Task)
    // so that call is a compile error. A call that doesn't compile can't be a test, so guard the shape instead.
    [Fact]
    public void Adapt_ShouldNotExposeSingleTypeArgumentShortcutOnPlainTask() =>
        Assert.DoesNotContain(
            typeof(Adapt).GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static),
            method => method.Name == nameof(Adapt.ToOutcome)
                      && method.IsGenericMethodDefinition
                      && method.GetGenericArguments().Length == 1
                      && method.GetParameters()[0].ParameterType == typeof(Task));
}
