using System;
using LibraryMembership.Database;
using LibraryMembership.Database.Repositories;
using LibraryMembership.Services;
using LibraryMembership.Slimmed;
using LibraryMembership.Slimmed.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(opts =>
{
    opts.UseInMemoryDatabase("SlidDownAggregates");
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<LibraryMembershipRepository>();
builder.Services.AddScoped<ILibraryMembershipService, LibraryMembershipService>();

WebApplication app = builder.Build();

app.MapAddLoanEndpoint();

app.Run();