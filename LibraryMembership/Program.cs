using LibraryMembership.Slimmed.Application.LibraryMembership;
using LibraryMembership.Slimmed.Domain.LibraryMembership.Abstractions;
using LibraryMembership.Slimmed.Infrastructure.Persistence;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Repositories;
using LibraryMembership.Slimmed.Presentation.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LibraryMembershipContext>(opts =>
{
    opts.UseInMemoryDatabase("LibraryMembership");
});

builder.Services.AddScoped<ILibraryMembershipRepository, LibraryMembershipRepository>();
builder.Services.AddScoped<ILibraryMembershipService, LibraryMembershipService>();

WebApplication app = builder.Build();

app.MapLoanBookEndpoint();
app.MapReturnBookEndpoint();

app.Run();