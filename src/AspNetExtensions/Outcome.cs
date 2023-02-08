using FluentValidation.Results;

namespace OsborneSupremacy.Extensions.AspNet;

public record Outcome<T> : IOutcome<T>
{
    public bool IsSuccess { get; }

    public bool IsFaulted => !IsSuccess;

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if this property is accessed when <see cref="IsSuccess"> is not true.</see></exception>
    public T Value =>
        !IsSuccess ? throw new InvalidOperationException("Cannot access value of outcome when not successful.") : _value!;

    private readonly T? _value;

    /// <exception cref="InvalidOperationException">Thrown if this property is accessed when <see cref="IsFaulted"> is not true.</see></exception>
    public Exception Exception =>
        IsSuccess ? throw new InvalidOperationException("Cannot access exception when successful.") : _exception!;

    private readonly Exception? _exception;

    public Outcome(T value)
    {
        _value = value;
        IsSuccess = true;
    }

    public Outcome(Exception exception)
    {
        _exception = exception;
        IsSuccess = false;
    }

    public IEnumerable<ValidationFailure> GetValidationErrors()
    {
        if (IsSuccess)
            throw new InvalidOperationException("Cannot access validation exceptions when successful.");

        if (_exception is not ValidationException validationException)
            return Enumerable.Empty<ValidationFailure>();

        return validationException.Errors;
    }
}
