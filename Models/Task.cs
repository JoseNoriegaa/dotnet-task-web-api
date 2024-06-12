namespace EntityFrameworkPracticeApp.Models;

internal class Task
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public Priority priority { get; set; }

    public Guid CategoryId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Category Category { get; set; }

}

public enum Priority
{
    LOW,
    MEDIUM,
    HIGH,
}
