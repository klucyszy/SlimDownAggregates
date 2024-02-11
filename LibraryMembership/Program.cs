using LibraryMembership.Database.Repositories;
using LibraryMembership.Services;
using LibraryMembership.Slimmed.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<LibraryMembershipRepository>();
builder.Services.AddScoped<ILibraryMembershipService, LibraryMembershipService>();

WebApplication app = builder.Build();

app.MapAddLoanEndpoint();

app.Run();