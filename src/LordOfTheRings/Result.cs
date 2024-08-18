namespace LordOfTheRings;
public class Result
{
    public string Error { get; }
    public bool IsSuccess { get; }

    public Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Failure(string error) => new(false, error);

    public static Result Success() => new(true, string.Empty);
}
public sealed class Result<T> : Result
{
    public T Value { get; }

    private Result(T value, bool isSuccess, string error)
        : base(isSuccess, error) => Value = value;

    public new static Result<T> Failure(string error) => new(default, false, error);

    public static Result<T> Success(T value) => new(value, true, string.Empty);
}