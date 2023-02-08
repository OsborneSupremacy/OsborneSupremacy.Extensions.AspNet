using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace OsborneSupremacy.Extensions.AspNet;

public static class ValidationExtensions
{
    public static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState)
    {
        if (result.IsValid) return;
        foreach (var error in result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            .Distinct()
        )
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
    }

    public static void AddValidationErrors(
        this ModelStateDictionary modelState,
        IEnumerable<ValidationFailure> validationFailures
        )
    {
        foreach (var failure in validationFailures)
            modelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
    }
}
