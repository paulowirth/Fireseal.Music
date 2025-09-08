using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Abstra.Challenge.Presentation.Middlewares;

public sealed class HttpContextExceptionMiddleware(RequestDelegate next, ILogger<HttpContextExceptionMiddleware> logger)
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = 
        new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true
        };
    
    private readonly Dictionary<Type, Func<HttpContext, Exception, ProblemDetails>> _exceptionHandlers = 
        new()
        {
            { typeof(Exception), HandleException },
            { typeof(ValidationException), HandleValidationException }
        };

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception, 
                "[{Middleware}] Caught Exception of Type [{ExceptionType}] on [{ExceptionSource}] with Message [{ExceptionMessage}]", 
                nameof(HttpContextExceptionMiddleware), exception.GetType(), exception.Source, exception.Message);
            
            await HandleExceptionAsync(httpContext, exception);
            
            logger.LogInformation(
                "[{Middleware}] Exception Handled Successfully", 
                nameof(HttpContextExceptionMiddleware));
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var problemDetails = 
            _exceptionHandlers.ContainsKey(exception.GetType()) 
                ? _exceptionHandlers[exception.GetType()].Invoke(httpContext, exception)
                : _exceptionHandlers[typeof(Exception)].Invoke(httpContext, exception);

        string httpResponseText = JsonSerializer.Serialize(problemDetails, _jsonSerializerOptions);
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        await httpContext.Response.WriteAsync(httpResponseText);
    }

    private static ProblemDetails HandleException(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        
        return new ProblemDetails
        {
            Detail = "An unexpected error occurred when processing the request.",
            Instance = httpContext.Request.Path,
            Status = httpContext.Response.StatusCode,
            Title = "HTTP 500 - Internal Server Error",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Extensions = 
                new Dictionary<string, object?>
                {
                    { 
                        "Exception", 
                        exception.InnerException is not null 
                            ? exception.InnerException.GetType().Name 
                            : exception.GetType().Name 
                    },
                    {
                        "Message", 
                        exception.InnerException is not null 
                            ? exception.InnerException.Message 
                            : exception.Message 
                    },
                    { "Source", exception.InnerException?.Source ?? exception.Source ?? string.Empty }
                }
        };
    }

    private static ProblemDetails HandleValidationException(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        
        var problemDetails = 
            new ProblemDetails
            {
                Title = "HTTP 400 - Bad Request",
                Detail = "One or more validation problems were found.",
                Instance = httpContext.Request.Path,
                Status = httpContext.Response.StatusCode,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

        if (exception is not ValidationException validationException)
            return problemDetails;
        
        var validationErrors = 
            validationException.Errors
                .GroupBy(
                    validationFailure => validationFailure.PropertyName, 
                    validationFailure => validationFailure.ErrorMessage)
                .ToDictionary(
                    group => group.Key, 
                    group => group.ToArray());
        
        problemDetails.Extensions.Add("validationErrors", validationErrors);
        
        return problemDetails;
    }
}