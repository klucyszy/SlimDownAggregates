using System;
using Microsoft.AspNetCore.Builder;

namespace LibraryMembership.Slimmed.Presentation.Endpoints;

public static class LibraryMembershipEndpoints
{
    public static void MapLoanBookEndpoint(this WebApplication app)
    {
        app.MapPost("api/library-membership/{id:guid}/operation/loan/{bookId:guid}", 
            async (Guid id, Guid bookId, ILibraryMembershipService service)
                => await service.LoanBookAsync(id, bookId));
    }
    
    public static void MapReturnBookEndpoint(this WebApplication app)
    {
        app.MapPost("api/library-membership/{id:guid}/operation/return/{bookId:guid}", 
            async (Guid id, Guid bookId, ILibraryMembershipService service)
                => await service.ReturnBookAsync(id, bookId));
    }
}