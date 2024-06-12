using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkPracticeApp.Models;

[Table("tasks")]
internal class Task
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    [Column("name")]
    public string Name { get; set; } = "";

    [Column("description")]
    public string Description { get; set; } = "";

    [Column("priority")]
    public Priority Priority { get; set; }

    [ForeignKey("Id")]
    [Column("category_id")]
    public Guid CategoryId { get; set; }

    [Column("created_at")]
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
