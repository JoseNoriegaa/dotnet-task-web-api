using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ApplicationDBContext : DbContext
{
    public DbSet<Models.Category> Categories { get; set; }
    public DbSet<Models.Task> Tasks { get; set; }

    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

    private static void ApplyCommonProperties<TEntity>(EntityTypeBuilder<TEntity> model) where TEntity : Models.BaseModel
    {
        model.HasKey(p => p.Id);

        model
            .Property(static p => p.Id)
            .HasColumnName("id");

        model
            .Property(static p => p.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        model
            .Property(static p => p.UpdatedAt)
            .HasColumnName("updated_at");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var categories = new List<Models.Category>()
        {
            new()
            {
                Id = Guid.Parse("e9d2de54-d048-42fd-8715-251875766097"),
                Name = "Actividades Pendientes",
                Weight = 20,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
            new()
            {
                Id = Guid.Parse("ea5fdb05-f70e-47e0-91ee-292f1baf5eae"),
                Name = "Actividades Personales",
                Weight = 50,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
        };

        modelBuilder.Entity<Models.Category>(model =>
        {
            model.ToTable("category");

            ApplyCommonProperties(model);

            model
                .Property(static p => p.Name)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("name");

            model
                .Property(static p => p.Description)
                .HasColumnName("description");

            model
                .Property(static p => p.Weight)
                .HasColumnName("weight");

            model.HasData(categories);
        });

        var tasks = new List<Models.Task>()
        {
            new()
            {
                Id = Guid.Parse("3c3fd5ab-2202-4dea-82ea-1ef1dc7c9b20"),
                Name = "Pago de servicios públicos",
                Description = null,
                CategoryId = categories[0].Id,
                Priority = Models.Priority.MEDIUM,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
            new()
            {
                Id = Guid.Parse("94cbd857-fd05-496a-b025-673815fdf7b8"),
                Name = "Terminar película en Netflix",
                Description = null,
                CategoryId = categories[1].Id,
                Priority = Models.Priority.LOW,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
        };

        modelBuilder.Entity<Models.Task>(model =>
        {
            model.ToTable("task");

            ApplyCommonProperties(model);

            model
                .Property(static p => p.Name)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("name");

            model
                .Property(static p => p.Description)
                .HasColumnName("description")
                .IsRequired(false);

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

            model.Ignore(static p => p.ShortDescription);

            model.HasData(tasks);
        });
    }
}
