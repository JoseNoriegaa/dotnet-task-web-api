namespace EntityFrameworkPracticeApp.DTOs;

public struct CategoryDTO
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int Weight { get; set; }
}
