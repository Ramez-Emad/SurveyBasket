using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mime;

namespace Presentation.Extensions;
public static class ValidatorExtensions
{
    public static async Task<IActionResult?> ValidateAsync<T>(
        this ControllerBase controller,
        T model,
        CancellationToken cancellationToken = default)
    {

        var validator = controller.HttpContext.RequestServices.GetService<IValidator<T>>();

        if (validator is null)
            return null; // No validation

        var result = await validator.ValidateAsync(model, cancellationToken);

        if (result.IsValid)
            return null;

        var errors = result.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        var problemDetails = new ValidationProblemDetails(errors)
        {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            Title = "One or more validation errors occurred.",
            Status = StatusCodes.Status400BadRequest,
        };

        return new ObjectResult(problemDetails)
        {
            StatusCode = StatusCodes.Status400BadRequest,
            ContentTypes = { MediaTypeNames.Application.Json, "application/problem+json" }
        };


    }
}