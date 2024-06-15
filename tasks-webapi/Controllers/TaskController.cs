using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace TasksWebApi.Controllers;

[ApiController]
[Route("api/tasks")]
[Produces("application/json")]
[Consumes("application/json")]
public class TaskController(
    ILogger<ITaskController> _logger,
    Services.ITaskService _taskService,
    Services.ICategoryService _categoryService) : ControllerBase, ITaskController
{
    // GET: api/tasks
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Models.Task>), 200)]
    [SwaggerOperation(Description = "Returns all the available tasks.")]
    public IActionResult List()
    {
        return Ok(_taskService.GetAllTasks());
    }

    // GET: api/tasks/{id}
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Models.Task), 200)]
    [SwaggerOperation(Description = "Returns the information of a task by its ID.")]
    public IActionResult Retrieve(Guid id)
    {
        var item = _taskService.GetTaskById(id);
        if (item == null)
        {
            return this.TaskNotFoundResponse(id);
        }

        return Ok(item);
    }

    // POST: api/tasks
    [HttpPost]
    [ProducesResponseType(typeof(Models.Task), 200)]
    [SwaggerOperation(Description = "Creates a new task.")]
    public IActionResult Create([FromBody] DTOs.TaskDTO body)
    {
        bool categoryExists = _categoryService.Exists(body.CategoryId);
        if (!categoryExists)
        {
            return this.CategoryNotFoundResponse(body.CategoryId);
        }

        Models.Task item = _taskService.CreateTask(body);

        return Ok(item);
    }

    // PUT: api/tasks/:id
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Models.Task), 200)]
    [SwaggerOperation(Description = "Updates a task by providing its ID and the data to update")]
    public IActionResult Update(Guid id, [FromBody] DTOs.TaskDTO body)
    {
        var item = _taskService.GetTaskById(id);
        if (item == null)
        {
            return this.TaskNotFoundResponse(id);
        }

        bool categoryExists = _categoryService.Exists(body.CategoryId);
        if (!categoryExists)
        {
            return this.CategoryNotFoundResponse(body.CategoryId);
        }

        _taskService.UpdateTask(item, body);

        return Ok(item);
    }

    // DELETE: api/tasks/:id
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Models.Task), 200)]
    [SwaggerOperation(Description = "Deletes a task by its ID")]
    public IActionResult Delete(Guid id)
    {
        var item = _taskService.GetTaskById(id);
        if (item == null)
        {
            return this.TaskNotFoundResponse(id);
        }

        _taskService.DeleteTask(item);

        return Ok(item);
    }

    private NotFoundObjectResult TaskNotFoundResponse(Guid id)
    {
        _logger.LogWarning("task was not found: {}", id);
        return NotFound(new DTOs.ApiMessageDto {
            Message = $"Task with ID '{id}' was not found",
        });
    }

    private BadRequestObjectResult CategoryNotFoundResponse(Guid id)
    {
        _logger.LogInformation("Category does not exists: {}", id);
        return BadRequest(new {
            CategoryId = new List<string>() { "Invalid category ID" },
        });
    }
}

public interface ITaskController
{
    public IActionResult List();
    public IActionResult Retrieve(Guid id);
    public IActionResult Create(DTOs.TaskDTO body);
    public IActionResult Update(Guid id, DTOs.TaskDTO body);
    public IActionResult Delete(Guid id);
}
