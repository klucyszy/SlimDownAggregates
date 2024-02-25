using System;
using System.Threading;
using LibraryMembership.Slimmed.Application.LibraryCart;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace LibraryMembership.Slimmed.Presentation.Endpoints;

public static class LibraryMembershipEndpoints
{
    public static void MapLoanBookEndpoint(this WebApplication app)
    {
        app.MapPost("api/membership/{id:guid}/books/{bookId:guid}/loan", 
            async (Guid id, Guid bookId, ILibraryCartService service, CancellationToken ct) =>
            {
                await service.LoanBookAsync(id, bookId, ct);

                return Results.Created();
            });
    }
    
    public static void MapReturnBookEndpoint(this WebApplication app)
    {
        app.MapPut("api/membership/{id:guid}/books/{bookId:guid}/return", 
            async (Guid id, Guid bookId, ILibraryCartService service, CancellationToken ct) =>
            {
                await service.ReturnBookAsync(id, bookId, ct);

                return Results.NoContent();
            });
    }
    
    public static void MapProlongBookLoanEndpoint(this WebApplication app)
    {
        app.MapPut("api/membership/{id:guid}/books/{bookId:guid}/prolong", 
            async (Guid id, Guid bookId, ILibraryCartService service, CancellationToken ct) =>
            {
                await service.ProlongBookLoanAsync(id, bookId, ct);

                return Results.NoContent();
            });
    }
}