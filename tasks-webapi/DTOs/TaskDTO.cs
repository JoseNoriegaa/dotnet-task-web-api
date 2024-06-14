namespace TasksWebApi.DTOs;

public struct TaskDTO
{
    public TaskDTO() {}

    public required string Name { get; set; }

    public string? Description { get; set; } = "";

    public required Guid CategoryId { get; set; }

    public Models.Priority? Priority { get; set; } = Models.Priority.LOW;
}
