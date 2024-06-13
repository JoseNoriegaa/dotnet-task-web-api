using Microsoft.AspNetCore.Mvc;
using Models = EntityFrameworkPracticeApp.Models;

namespace EntityFrameworkPracticeApp.Controllers;

[ApiController]
[Route("api/tasks")]
public class TaskController(ApplicationDBContext dbContext, ILogger<TaskController> logger) : ControllerBase
{
    // GET: api/tasks
    [HttpGet]
    public IActionResult List()
    {
        logger.LogInformation("fetching all tasks");
        return Ok(dbContext.Tasks);
    }

    // GET: api/tasks/{id}
    [HttpGet("{id:guid}")]
    public IActionResult Retrieve(Guid id)
    {
        logger.LogInformation("fetching tasks with ID: {}", id);
        var item = dbContext.Tasks.Find(id);

        if (item == null)
        {
            logger.LogWarning("task was not found: {}", id);
            return NotFound(new {
                Message = $"Task with ID '{id}' was not found",
            });
        }

        return Ok(item);
    }

    // POST: api/tasks
    [HttpPost]
    public IActionResult Create([FromBody] TaskDTO body)
    {
        logger.LogInformation("creating new task");

        logger.LogInformation("validating category ID: {CategoryId}", body.CategoryId);
        var categoryExists = dbContext.Categories.Any(p => p.Id == body.CategoryId);
        if (!categoryExists)
        {
            logger.LogInformation("the provided category does not exists: {}", body.CategoryId);
            return BadRequest(new {
                CategoryId = new List<string>() { "Invalid category ID" },
            });
        }

        Models.Task task = new() {
            Name = body.Name,
            Description = body.Description,
            CategoryId = body.CategoryId,
            Priority = body.Priority ?? Models.Priority.LOW,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Id = Guid.NewGuid(),
        };


        logger.LogInformation("saving new task: {}", task.Id);
        dbContext.Add(task);
        dbContext.SaveChanges();

        return Ok(task);
    }

    // PUT: api/tasks/:id
    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, [FromBody] TaskDTO body)
    {
        logger.LogInformation("fetching tasks with ID: {}", id);
        var item = dbContext.Tasks.Find(id);

        if (item == null)
        {
            logger.LogWarning("task was not found: {}", id);
            return NotFound(new {
                Message = $"Task with ID '{id}' was not found",
            });
        }

        logger.LogInformation("validating category ID: {CategoryId}", body.CategoryId);
        var categoryExists = dbContext.Categories.Any(p => p.Id == body.CategoryId);
        if (!categoryExists)
        {
            logger.LogInformation("the provided category does not exists: {}", body.CategoryId);
            return BadRequest(new {
                CategoryId = new List<string>() { "Invalid category ID" },
            });
        }

        item.Name = body.Name;
        item.Description = body.Description;
        item.CategoryId = body.CategoryId;
        item.Priority = body.Priority ?? Models.Priority.LOW;

        logger.LogInformation("updating task: {}", id);
        dbContext.SaveChanges();

        return Ok(item);
    }

    // DELETE: api/tasks/:id
    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        logger.LogInformation("fetching tasks with ID: {}", id);
        var item = dbContext.Tasks.Find(id);

        if (item == null)
        {
            logger.LogWarning("task was not found: {}", id);
            return NotFound(new {
                Message = $"Task with ID '{id}' was not found",
            });
        }

        logger.LogInformation("removing task: {}", id);
        dbContext.Remove(item);
        dbContext.SaveChanges();

        return Ok(item);
    }
}

public struct TaskDTO
{
    public TaskDTO() {}

    public required string Name { get; set; }

    public string? Description { get; set; } = "";

    public required Guid CategoryId { get; set; }

    public Models.Priority? Priority { get; set; } = Models.Priority.LOW;
}
