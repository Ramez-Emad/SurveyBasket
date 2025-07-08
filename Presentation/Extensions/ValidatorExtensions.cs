using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Extensions;
public static class ValidatorExtensions
{
    public static async Task<IActionResult?> ValidateAsync<T>(
        this ControllerBase controller,
        T model,
        IValidator<T> validator,
        CancellationToken cancellationToken = default)
    {
        var result = await validator.ValidateAsync(model, cancellationToken);

        if (result.IsValid)
            return null;

        var groupedErrors = result.Errors
            .GroupBy(e => e.PropertyName)
            .Select(g => new ValidationError
            {
                Field = g.Key,
                Errors = g.Select(e => e.ErrorMessage)
            });

        var errorResponse = new ValidationErrorToReturn
        {
            Errors = groupedErrors.ToList()
        };

        return new BadRequestObjectResult(errorResponse);
    }
}