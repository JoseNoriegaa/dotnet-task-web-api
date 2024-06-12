using Microsoft.EntityFrameworkCore;

using Models = EntityFrameworkPracticeApp.Models;

internal class TaskDBContext: DbContext {
    public DbSet<EntityFrameworkPracticeApp.Models.Category> Categories {get; set; }
    public DbSet<EntityFrameworkPracticeApp.Models.Task> Tasks { get; set; }

    public TaskDBContext(DbContextOptions<TaskDBContext> options): base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.Category>(static model => {
            model.ToTable("category");
            model.HasKey(static p => p.Id);

            model
                .Property(static p => p.Id)
                .HasColumnName("id");

            model
                .Property(static p => p.Name)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("name");

            model
                .Property(static p => p.Description)
                .HasColumnName("description");
        });

        modelBuilder.Entity<Models.Task>(static model => {
            model.ToTable("task");

            model
                .Property(static p => p.Id)
                .HasColumnName("id");

            model
                .Property(static p => p.Name)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("name");

            model
                .Property(static p => p.Description)
                .HasColumnName("description");

            model
                .Property(static p => p.Priority)
                .HasColumnName("priority");

            model
                .Property(static p => p.CategoryId)
                .HasColumnName("category_id");

            model
                .HasOne(static p => p.Category)
                .WithMany(static p => p.Tasks)
                .HasForeignKey(static p => p.CategoryId);

            model
                .Property(static p => p.CreatedAt)
                .HasColumnName("created_at");

            model.Ignore(static p => p.ShortDescription);
        });
    }
}
