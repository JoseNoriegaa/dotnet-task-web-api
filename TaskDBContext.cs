using Microsoft.EntityFrameworkCore;

internal class TaskDBContext: DbContext {
    public DbSet<EntityFrameworkPracticeApp.Models.Category> Categories {get; set; }
    public DbSet<EntityFrameworkPracticeApp.Models.Task> Tasks { get; set; }

    public TaskDBContext(DbContextOptions<TaskDBContext> options): base(options) {}
}
