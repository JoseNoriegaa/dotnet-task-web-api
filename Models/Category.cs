using System.Text.Json.Serialization;

namespace TasksWebApi.Models;

public class Category : BaseModel
{
    public string Name { get; set; } = "";

    public string Description { get; set; } = "";

    public int Weight { get; set; }

    [JsonIgnore]
    public virtual ICollection<Task> Tasks { get; set; } = [];
}
