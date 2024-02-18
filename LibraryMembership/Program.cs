using LibraryMembership.Slimmed.Application.LibraryMembership;
using LibraryMembership.Slimmed.Domain.LibraryMembership.Abstractions;
using LibraryMembership.Slimmed.Infrastructure.Persistence;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Repositories;
using LibraryMembership.Slimmed.Presentation.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Wolverine;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine();

builder.Services.AddDatabase();
builder.Services.AddScoped<ILibraryMembershipRepository, LibraryMembershipRepository>();
builder.Services.AddScoped<ILibraryMembershipService, LibraryMembershipService>();

WebApplication app = builder.Build();

app.MapLoanBookEndpoint();
app.MapReturnBookEndpoint();
app.MapReserveBookEndpoint();
app.MapCancelBookReservationEndpoint();

app.Run();