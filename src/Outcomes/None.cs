namespace WarpCode.Outcomes;

/// <summary>
/// Primitive representing no value, like <see cref="System.Void"/>.
/// </summary>
public readonly struct None
{
}

public static class Scratch
{
    public static Result<int> SillyExample(int firstInput) =>
        Result.Of(firstInput)
        | NextInput(13)
        | AndAnotherInput(42)
        | AddThem;

    
    private static Func<int, (int, int)> NextInput(int next) => last => (last, next);
    private static Func<(int, int), (int, int, int)> AndAnotherInput(int next) => last => (last.Item1, last.Item2, next);
    private static int AddThem(int a, int b, int c) => a + b + c;
}