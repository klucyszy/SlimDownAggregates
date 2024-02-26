using System;
using System.Threading;
using LibraryMembership.Slimmed.Application.LibraryCart;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace LibraryMembership.Slimmed.Presentation.Endpoints;

public static class LibraryEndpoints
{
    public static void MapLoanBookEndpoint(this WebApplication app)
    {
        app.MapPost("api/membership/{id:guid}/books/{bookIsbn}/loan", 
            async (Guid id, string bookIsbn, ILibraryCartService service, CancellationToken ct) =>
            {
                await service.LoanBookAsync(id, bookIsbn, ct);

                return Results.Created();
            });
    }
    
    public static void MapReturnBookEndpoint(this WebApplication app)
    {
        app.MapPut("api/membership/{id:guid}/loans/{bookId:guid}/return", 
            async (Guid id, Guid bookId, ILibraryCartService service, CancellationToken ct) =>
            {
                await service.ReturnBookAsync(id, bookId, ct);

                return Results.NoContent();
            });
    }
    
    public static void MapProlongBookLoanEndpoint(this WebApplication app)
    {
        app.MapPut("api/membership/{id:guid}/loans/{bookId:guid}/prolong", 
            async (Guid id, Guid bookId, ILibraryCartService service, CancellationToken ct) =>
            {
                await service.ProlongBookLoanAsync(id, bookId, ct);

                return Results.NoContent();
            });
    }
}