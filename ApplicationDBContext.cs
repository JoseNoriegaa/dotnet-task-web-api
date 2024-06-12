using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models = EntityFrameworkPracticeApp.Models;

internal class ApplicationDBContext: DbContext {
    public DbSet<Models.Category> Categories {get; set; }
    public DbSet<Models.Task> Tasks { get; set; }

    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options): base(options) {}

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

        modelBuilder.Entity<Models.Category>(static model => {
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
        });

        modelBuilder.Entity<Models.Task>(static model => {
            model.ToTable("task");

            ApplyCommonProperties(model);

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

            model.Ignore(static p => p.ShortDescription);
        });
    }
}
