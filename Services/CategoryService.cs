namespace TasksWebApi.Services;

public class CategoryService(ApplicationDBContext _dbContext, ILogger<CategoryService> _logger) : ICategoryService
{

    public IEnumerable<Models.Category> GetAllCategories()
    {
        _logger.LogInformation("Fetching all categories");
        return _dbContext.Categories;
    }

    public Models.Category? GetCategoryById(Guid id)
    {
        _logger.LogInformation("Fetching category with ID: {Id}", id);
        return _dbContext.Categories.Find(id);
    }

    public Models.Category CreateCategory(DTOs.CategoryDTO categoryDto)
    {
        _logger.LogInformation("Creating new category");
        Models.Category item = new() {
            Id = Guid.NewGuid(),
            Name = categoryDto.Name,
            Description = categoryDto.Description ?? "",
            Weight = categoryDto.Weight,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        _logger.LogInformation("Saving new category: {}", item.Id);
        _dbContext.Add(item);
        _dbContext.SaveChanges();

        return item;
    }

    public void UpdateCategory(Models.Category category, DTOs.CategoryDTO categoryDto)
    {
        _logger.LogInformation("Updating category: {Id}", category.Id);
        category.Name = categoryDto.Name;
        category.Description = categoryDto.Description ?? "";
        category.Weight = categoryDto.Weight;
        category.UpdatedAt = DateTime.UtcNow;

        _logger.LogInformation("Saving updated category: {}", category.Id);
        _dbContext.SaveChanges();
    }

    public void DeleteCategory(Models.Category category)
    {
        _logger.LogInformation("Removing task: {Id}", category.Id);
        _dbContext.Remove(category);
        _dbContext.SaveChanges();
    }

    public int CountRelatedTasks(Guid categoryId)
    {
        _logger.LogInformation("Counting tasks related to category with Id: {Id}", categoryId);
        return _dbContext.Tasks.Count(p => p.CategoryId == categoryId);
    }

    public bool Exists(Guid id)
    {
        _logger.LogInformation("Verifying if category exists: {Id}", id);
        return _dbContext.Categories.Any(p => p.Id == id);
    }
}

public interface ICategoryService
{
    public IEnumerable<Models.Category> GetAllCategories();
    public Models.Category? GetCategoryById(Guid id);
    public Models.Category CreateCategory(DTOs.CategoryDTO categoryDto);
    public void UpdateCategory(Models.Category category, DTOs.CategoryDTO categoryDto);
    public void DeleteCategory(Models.Category category);
    public int CountRelatedTasks(Guid categoryId);

    public bool Exists(Guid id);
}
