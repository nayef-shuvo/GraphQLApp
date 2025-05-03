namespace GraphQLApp.Common;

public record Result
{
    public bool IsSuccess { get; }
    public string? Error { get; }

    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, string? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(string error) => new(false, error);

    public void Deconstruct(out bool isSuccess, out string? error)
        => (isSuccess, error) = (IsSuccess, Error);
}

public sealed record Result<T> : Result
{
    public T? Value { get; }

    private Result(T value) : base(true, null)
        => Value = value;

    private Result(string error) : base(false, error)
        => Value = default;

    public static Result<T> Success(T value) => new(value);
    public new static Result<T> Failure(string error) => new(error);
    public static implicit operator Result<T>(T value) => Success(value);

    public void Deconstruct(out bool isSuccess, out T? value, out string? error)
    {
        isSuccess = IsSuccess;
        value = Value;
        error = Error;
    }
}