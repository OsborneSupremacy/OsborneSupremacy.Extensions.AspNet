using FluentValidation.Results;

namespace OsborneSupremacy.Extensions.AspNet;

/// <summary>
/// This is an unapologetic pale immitation of Result from LanguageExt.Common. It doesn't
/// attempt to do anything close to what that class does. That class is great, however
/// if you're not in the functional programming mindset, or your application isn't written
/// with a functional approach, it might not be the best fit.
/// 
/// The elements I borrowed from that class are:
/// 
/// 1. Encapsulating the result of an operation, with <see cref="IsSuccess"/> and <see cref="IsFaulted"/> properties
/// used to evaulate whether the operation succeeded.
/// 
/// 2. An <see cref="Exception"/> property to hold the details of the failure, when the operation fails.
/// 
/// Since I use this class alot for validation, I wanted any easy way to get validation errors, 
/// hence the <see cref="GetValidationErrors"/> method.
/// 
/// I will most likely expand it with methods to make it easier to get to the details or other types 
/// of extensions.
/// </summary>
/// <typeparam name="T"></typeparam>
public record Outcome<T>
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

    public static implicit operator Outcome<T>(T value) => new(value);
}

public record Outcome
{
    public bool IsSuccess { get; }

    public bool IsFaulted => !IsSuccess;

    /// <exception cref="InvalidOperationException">Thrown if this property is accessed when <see cref="IsFaulted"> is not true.</see></exception>
    public Exception Exception =>
        IsSuccess ? throw new InvalidOperationException("Cannot access exception when successful.") : _exception!;

    private readonly Exception? _exception;

    public Outcome()
    {
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
