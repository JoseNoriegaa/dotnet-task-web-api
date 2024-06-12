using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkPracticeApp.Models;

internal class Category
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = "";

    public string Description { get; set; } = "";

    public virtual ICollection<Task> Tasks { get; set; } = [];
}
