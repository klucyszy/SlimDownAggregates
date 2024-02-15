using System;
using System.Threading;
using LibraryMembership.Slimmed.Application.LibraryMembership;
using Microsoft.AspNetCore.Builder;

namespace LibraryMembership.Slimmed.Presentation.Endpoints;

public static class LibraryMembershipEndpoints
{
    public static void MapLoanBookEndpoint(this WebApplication app)
    {
        app.MapPost("api/library-membership/{id:guid}/operation/loan/{bookId:guid}", 
            async (Guid id, Guid bookId, ILibraryMembershipService service, CancellationToken ct)
                => await service.LoanBookAsync(id, bookId, ct));
    }
    
    public static void MapReturnBookEndpoint(this WebApplication app)
    {
        app.MapPost("api/library-membership/{id:guid}/operation/return/{bookId:guid}", 
            async (Guid id, Guid bookId, ILibraryMembershipService service, CancellationToken ct)
                => await service.ReturnBookAsync(id, bookId, ct));
    }
    
    public static void MapReserveBookEndpoint(this WebApplication app)
    {
        app.MapPost("api/library-membership/{id:guid}/operation/reserve/{bookId:guid}", 
            async (Guid id, Guid bookId, ILibraryMembershipService service, CancellationToken ct)
                => await service.ReserveBookAsync(id, bookId, ct));
    }
    
    public static void MapCancelBookReservationEndpoint(this WebApplication app)
    {
        app.MapPost("api/library-membership/{id:guid}/operation/cancel-reservation/{bookId:guid}", 
            async (Guid id, Guid bookId, ILibraryMembershipService service, CancellationToken ct)
                => await service.CancelBookReservationAsync(id, bookId, ct));
    }
}