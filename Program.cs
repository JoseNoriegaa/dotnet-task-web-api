using EntityFrameworkPracticeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Models = EntityFrameworkPracticeApp.Models;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddNpgsql<ApplicationDBContext>(builder
        .Configuration
        .GetConnectionString("TasksDatabase"));

var app = builder.Build();

// Tasks
app.MapGet("/api/tasks", static ([FromServices] ApplicationDBContext ctx) =>
{
    return Results.Ok(ctx.Tasks);
});

app.MapGet("/api/tasks/{id:guid}", static ([FromServices] ApplicationDBContext ctx, Guid id) =>
{
    var item = ctx.Tasks.Find(id);

    if (item == null)
    {
        return Results.NotFound(new
        {
            Message = $"Task with ID \"${id}\" was not found",
        });
    }

    return Results.Ok(item);
});

app.MapPost("/api/tasks", ([FromServices] ApplicationDBContext ctx, [FromBody] TaskDTO body) => {

    var categoryExists = ctx.Categories.Any(p => p.Id == body.CategoryId);

    if (!categoryExists) {
        return Results.BadRequest(new {
            CategoryId = new List<string>() { "Invalid category" },
        });
    }

    Models.Task task = new() {
        Id = Guid.NewGuid(),
        Name = body.Name,
        Description = body.Description,
        CategoryId = body.CategoryId,
        Priority = body.Priority,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
    };

    ctx.Add(task);

    ctx.SaveChanges();

    return Results.Ok(task);
});

app.MapPut("/api/tasks/{id:guid}", (Guid id, [FromServices] ApplicationDBContext ctx, [FromBody] TaskDTO body) => {
    var item = ctx.Tasks.Find(id);

    if (item == null)
    {
        return Results.NotFound(new
        {
            Message = $"Task with ID \"${id}\" was not found",
        });
    }

    var categoryExists = ctx.Categories.Any(p => p.Id == body.CategoryId);

    if (!categoryExists) {
        return Results.BadRequest(new {
            CategoryId = new List<string>() { "Invalid category" },
        });
    }

    item.Name = body.Name;
    item.Description = body.Description;
    item.CategoryId = body.CategoryId;
    item.Priority = body.Priority;
    item.UpdatedAt = DateTime.UtcNow;

    ctx.SaveChanges();

    return Results.Ok(item);
});

app.MapDelete("/api/tasks/{id:guid}", (Guid id, [FromServices] ApplicationDBContext ctx) => {
    var item = ctx.Tasks.Find(id);

    if (item == null)
    {
        return Results.NotFound(new
        {
            Message = $"Task with ID \"${id}\" was not found",
        });
    }

    ctx.Tasks.Remove(item);

    ctx.SaveChanges();

    return Results.Ok(item);
});

// Categories
app.MapGet("/api/categories", static ([FromServices] ApplicationDBContext ctx) => {
    return Results.Ok(ctx.Categories);
});

app.MapGet("/api/categories/{id:guid}", static ([FromServices] ApplicationDBContext ctx, Guid id) => {
    var item = ctx.Categories.Find(id);

    if (item == null)
    {
        return Results.NotFound(new
        {
            Message = $"Category with ID \"${id}\" was not found",
        });
    }

    return Results.Ok(item);
});

app.Run();

struct TaskDTO
{
    public required string Name { get; set; }
    public string Description { get; set; } = "";
    public required Guid CategoryId { get; set; }
    public Priority Priority { get; set; } = Priority.LOW;
    public TaskDTO() {}
}
