namespace EntityFrameworkPracticeApp.Models;

internal class Category : BaseModel
{
    public string Name { get; set; } = "";

    public string Description { get; set; } = "";

    public virtual ICollection<Task> Tasks { get; set; } = [];

    public int Weight { get; set; }
}
