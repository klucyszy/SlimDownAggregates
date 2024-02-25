using System.Reflection;
using LibraryMembership.Shared.Infrastructure.Abstractions;
using LibraryMembership.Slimmed.Application.LibraryCart;
using LibraryMembership.Slimmed.Application.LibraryMembership;
using LibraryMembership.Slimmed.Infrastructure.Persistence;
using LibraryMembership.Slimmed.Presentation.Endpoints;
using LibraryMembership.Slimmed.Presentation.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LibraryMembershipContext>(opts =>
{
    opts.UseInMemoryDatabase("LibraryMembership");
});

builder.Services.AddAggregateRepositories(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<ILibraryCartService, LibraryCartService>();
builder.Services.AddScoped<ILibraryMembershipService, LibraryMembershipService>();

WebApplication app = builder.Build();

app.UseExceptionFilterMiddleware();

app.MapLoanBookEndpoint();
app.MapReturnBookEndpoint();
app.MapProlongBookLoanEndpoint();

app.Run();