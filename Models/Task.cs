using System.Text.Json.Serialization;

namespace TasksWebApi.Models;

public class Task : BaseModel
{
    public string Name { get; set; } = "";
    public string? Description { get; set; }

    public Priority Priority { get; set; }

    public Guid CategoryId { get; set; }

    [JsonIgnore]
    public virtual Category? Category { get; set; }

    public string? ShortDescription { get; set; }
}

public enum Priority
{
    LOW,
    MEDIUM,
    HIGH,
}
