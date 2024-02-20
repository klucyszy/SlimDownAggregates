using System;
using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMembership.Slimmed.Presentation.Middleware;

public static class ExceptionFilterMiddleware
{
    public static void UseExceptionFilterMiddleware(this WebApplication app)
    {
        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async (context) =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = MediaTypeNames.Application.Json;

                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                Exception? exception = exceptionHandlerPathFeature?.Error;

                if (exception is not null)
                {
                    await context.Response.WriteAsJsonAsync(
                        new ProblemDetails
                        {
                            Detail = exception.Message
                        });
                }
                else
                {
                    await context.Response.WriteAsJsonAsync(
                        new ProblemDetails
                        {
                            Detail = "Something went wrong. Please try again later."
                        });
                }
            });
        });
    }
}