﻿using Microsoft.AspNetCore.Builder;

namespace OsborneSupremacy.Extensions.AspNet;

public static class WebApplicationBuilderExtensions
{
    public static T GetTypedSection<T>(
        this WebApplicationBuilder input,
        string sectionName
    ) where T : new()
    {
        T output = new();
        input.Configuration.GetSection(sectionName)
            .Bind(output);
        return output;
    }

    public static T GetAndValidateTypedSection<T>(
        this WebApplicationBuilder input,
        string sectionName,
        AbstractValidator<T> validator
    ) where T : new()
    {
        var output = input.GetTypedSection<T>(sectionName);

        var validationResult = validator.Validate(output);

        if (!validationResult.IsValid)
            throw new AggregateException(
                validationResult
                    .Errors
                    .Select(x => new ValidationException(x.ErrorMessage))
            );

        return output;
    }
}
