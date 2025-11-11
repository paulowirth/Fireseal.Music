using System.Text.Json;
using Fireseal.Music.Presentation.Middlewares;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Fireseal.Music.Tests.Unit.Presentation.Middlewares;

public sealed class HttpContextExceptionMiddlewareTests
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly ILogger<HttpContextExceptionMiddleware> _logger = 
        Substitute.For<ILogger<HttpContextExceptionMiddleware>>();
    
    [Fact]
    public async Task InvokeAsync_CallsNextDelegate_WhenNoExceptionIsThrown()
    {
        //Arrange
        var context = new DefaultHttpContext();
        var wasCalled = false;
        
        RequestDelegate next = _ =>
        {
            wasCalled = true; 
            return Task.CompletedTask; 
        };
        
        var middleware = new HttpContextExceptionMiddleware(next, _logger);

        //Act
        await middleware.InvokeAsync(context);

        //Assert
        Assert.True(wasCalled);
        Assert.Equal(200, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_HandleException_ReturnsExpected()
    {
        //Arrange
        var context = new DefaultHttpContext();
        RequestDelegate next = _ => throw new Exception("Test error");
        
        var middleware = new HttpContextExceptionMiddleware(next, _logger);
        var responseBody = new MemoryStream();
        context.Response.Body = responseBody;
        context.Request.Path = "/test";

        //Act
        await middleware.InvokeAsync(context);
        responseBody.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(responseBody).ReadToEndAsync();
        var problem = JsonSerializer.Deserialize<ProblemDetails>(responseText, _jsonSerializerOptions);

        //Assert
        Assert.Equal(500, context.Response.StatusCode);
        Assert.NotNull(problem);
        Assert.Equal(context.Request.Path, problem.Instance);
        Assert.Equal(500, problem.Status);
    }

    [Fact]
    public async Task InvokeAsync_HandleValidationException_ReturnsExpected()
    {
        //Arrange
        var context = new DefaultHttpContext();
        var validationFailures = 
            new[]
            {
                new ValidationFailure("Name", "Name is required"),
                new ValidationFailure("Age", "Age must be positive")
            };
        
        var validationException = new ValidationException(validationFailures);
        RequestDelegate next = _ => throw validationException;
        var middleware = new HttpContextExceptionMiddleware(next, _logger);
        var responseBody = new MemoryStream();
        context.Response.Body = responseBody;
        context.Request.Path = "/test-validation";

        //Act
        await middleware.InvokeAsync(context);
        responseBody.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(responseBody).ReadToEndAsync();
        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(responseText, _jsonSerializerOptions);

        //Assert
        Assert.NotNull(problemDetails);
        Assert.Equal(context.Request.Path, problemDetails.Instance);
        Assert.Equal(400, problemDetails.Status);
        Assert.Equal(400, context.Response.StatusCode);
        Assert.NotEmpty(problemDetails.Extensions);
        Assert.True(problemDetails.Extensions.ContainsKey("validationErrors"));
    }
}