using System;
using LibraryMembership.Services;
using Microsoft.AspNetCore.Builder;

namespace LibraryMembership.Slimmed.Endpoints;

public static class AddLoanEndpoint
{
    public static void MapAddLoanEndpoint(this WebApplication app)
    {
        app.MapPost("api/library-membership/{id:guid}/operation/loan/{bookId:guid}", 
            async (Guid id, Guid bookId, ILibraryMembershipService service)
                => await service.AddLoanAsync(id, bookId));
    }
}