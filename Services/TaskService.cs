namespace EntityFrameworkPracticeApp.Services;

public class TaskService(ApplicationDBContext _dbContext, ILogger<TaskService> _logger) : ITaskService
{
    public IEnumerable<Models.Task> GetAllTasks()
    {
        _logger.LogInformation("Fetching all tasks");
        return _dbContext.Tasks;
    }

    public Models.Task? GetTaskById(Guid id)
    {
        _logger.LogInformation("Fetching task with ID: {Id}", id);
        return _dbContext.Tasks.Find(id);
    }

    public Models.Task CreateTask(DTOs.TaskDTO taskDto)
    {
        _logger.LogInformation("Creating new task");
        Models.Task task = new() {
            Name = taskDto.Name,
            Description = taskDto.Description,
            CategoryId = taskDto.CategoryId,
            Priority = taskDto.Priority ?? Models.Priority.LOW,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Id = Guid.NewGuid(),
        };

        _logger.LogInformation("Saving new task: {Id}", task.Id);
        _dbContext.Add(task);
        _dbContext.SaveChanges();
        return task;
    }

    public void UpdateTask(Models.Task task, DTOs.TaskDTO taskDto)
    {
        _logger.LogInformation("Updating task: {Id}", task.Id);
        task.Name = taskDto.Name;
        task.Description = taskDto.Description;
        task.CategoryId = taskDto.CategoryId;
        task.Priority = taskDto.Priority ?? Models.Priority.LOW;
        task.UpdatedAt = DateTime.UtcNow;

        _logger.LogInformation("Saving updated task: {Id}", task.Id);
        _dbContext.SaveChanges();
    }

    public void DeleteTask(Models.Task task)
    {
        _logger.LogInformation("Removing task: {Id}", task.Id);
        _dbContext.Remove(task);
        _dbContext.SaveChanges();
    }

}

public interface ITaskService
{
    public IEnumerable<Models.Task> GetAllTasks();
    public Models.Task? GetTaskById(Guid id);
    public Models.Task CreateTask(DTOs.TaskDTO taskDto);
    public void UpdateTask(Models.Task task, DTOs.TaskDTO taskDto);
    public void DeleteTask(Models.Task task);
}
