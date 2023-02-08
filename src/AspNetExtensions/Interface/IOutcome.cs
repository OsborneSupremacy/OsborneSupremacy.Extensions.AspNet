using FluentValidation.Results;

namespace OsborneSupremacy.Extensions.AspNet;

public interface IOutcome<T>
{
    public bool IsSuccess { get; }

    public bool IsFaulted { get; }

    public T Value { get; }

    public Exception Exception { get; }

    public IEnumerable<ValidationFailure> GetValidationErrors();
}
