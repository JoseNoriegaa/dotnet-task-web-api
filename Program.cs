using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddNpgsql<ApplicationDBContext>(builder
        .Configuration
        .GetConnectionString("TasksDatabase"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/db-connection", static async ([FromServices] ApplicationDBContext dbCtx) =>
{
    var result = await dbCtx.Database.EnsureCreatedAsync();

    return Results.Ok($"Database status (created, in-memory): {result}, {dbCtx.Database.IsNpgsql()}");
});

app.Run();
