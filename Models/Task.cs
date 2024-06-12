using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkPracticeApp.Models;

internal class Task
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";

    public Priority Priority { get; set; }

    public Guid CategoryId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Category? Category { get; set; }

    [NotMapped]
    public string ShortDescription { get; set; } = "";

}

public enum Priority
{
    LOW,
    MEDIUM,
    HIGH,
}
