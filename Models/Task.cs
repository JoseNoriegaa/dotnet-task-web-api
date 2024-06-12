using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkPracticeApp.Models;

internal class Task
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = "";

    public string Description { get; set; } = "";

    public Priority Priority { get; set; }

    [ForeignKey("Id")]
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
