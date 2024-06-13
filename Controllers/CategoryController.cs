using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkPracticeApp.Controllers;

[Route("api/categories")]
[ApiController]
public class CategoryController([FromServices] ApplicationDBContext dbContext, ILogger<CategoryController> logger) : ControllerBase, ICategoryController
{
    // GET: api/categories
    [HttpGet]
    public IActionResult List()
    {
        logger.LogInformation("fetching and returning all the categories");
        return Ok(dbContext.Categories);
    }

    // GET: api/categories/:id
    [HttpGet("{id:guid}")]
    public IActionResult Retrieve(Guid id)
    {
        logger.LogInformation("fetching category with ID: {}", id);
        var item = dbContext.Categories.Find(id);

        if (item == null) {
            logger.LogWarning("category was not found: {}", id);

            return NotFound(new {
                Message = $"Category with ID '{id}' was not found",
            });
        }

        return Ok(item);
    }

    // POST: api/categories
    [HttpPost]
    public IActionResult Create([FromBody] CategoryDTO body)
    {
        logger.LogInformation("creating new category");
        Models.Category item = new() {
            Id = Guid.NewGuid(),
            Name = body.Name,
            Description = body.Description ?? "",
            Weight = body.Weight,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        logger.LogInformation("saving new category: {}", item.Id);
        dbContext.Add(item);
        dbContext.SaveChanges();

        return Ok(item);
    }

    // PUT: api/categories/:id
    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, CategoryDTO body)
    {
        var item = this.FetchCategoryById(id);
        if (item == null) {
            return this.CategoryNotFoundResponse(id);
        }

        item.Name = body.Name;
        item.Description = body.Description ?? "";
        item.Weight = body.Weight;
        item.UpdatedAt = DateTime.UtcNow;

        logger.LogInformation("updating category: {}", item.Id);
        dbContext.SaveChanges();

        return Ok(item);
    }

    // DELETE: api/categories/:id
    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var item = this.FetchCategoryById(id);
        if (item == null) {
            return this.CategoryNotFoundResponse(id);
        }

        var count = dbContext.Tasks.Count(p => p.CategoryId == id);
        if (count != 0)
        {
            var msg = "Cannot delete a category with associated tasks.";
            logger.LogWarning(msg);
            return Conflict(new {
                Message = msg,
            });
        }

        logger.LogInformation("deleting category: {}", item.Id);
        dbContext.Remove(item);
        dbContext.SaveChanges();

        return Ok(count);
    }

    private Models.Category? FetchCategoryById(Guid id)
    {
        return dbContext.Categories.Find(id);
    }

    private NotFoundObjectResult CategoryNotFoundResponse(Guid id)
    {
        logger.LogWarning("category was not found: {}", id);
        return NotFound(new {
            Message = $"Category with ID '{id}' was not found",
        });
    }
}

public struct CategoryDTO
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int Weight { get; set; }
}

public interface ICategoryController
{
    public IActionResult List();
    public IActionResult Retrieve(Guid id);
    public IActionResult Create(CategoryDTO body);
    public IActionResult Update(Guid id, CategoryDTO body);
    public IActionResult Delete(Guid id);
}
