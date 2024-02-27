using System;
using System.Threading;
using LibraryMembership.Shared;
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
                Result<Guid> result = await service.LoanBookAsync(id, bookIsbn, ct);

                return Results.Created($"api/membership/{id}/loans/{result.Value}", result.Value);
            });
    }
    
    public static void MapReturnBookEndpoint(this WebApplication app)
    {
        app.MapPut("api/membership/{id:guid}/loans/{loanId:guid}/return", 
            async (Guid id, Guid loanId, ILibraryCartService service, CancellationToken ct) =>
            {
                await service.ReturnBookAsync(id, loanId, ct);

                return Results.NoContent();
            });
    }
    
    public static void MapProlongBookLoanEndpoint(this WebApplication app)
    {
        app.MapPut("api/membership/{id:guid}/loans/{loanId:guid}/prolong", 
            async (Guid id, Guid loanId, ILibraryCartService service, CancellationToken ct) =>
            {
                await service.ProlongBookLoanAsync(id, loanId, ct);

                return Results.NoContent();
            });
    }
}