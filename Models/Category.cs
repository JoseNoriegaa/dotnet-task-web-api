using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkPracticeApp.Models;

[Table("categories")]
internal class Category
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(150)]
    [Column("name")]
    public string Name { get; set; } = "";

    [Column("description")]
    public string Description { get; set; } = "";

    public virtual ICollection<Task> Tasks { get; set; } = [];
}
