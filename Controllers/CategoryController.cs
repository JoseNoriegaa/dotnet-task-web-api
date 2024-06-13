using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkPracticeApp.Controllers;

[Route("api/categories")]
[ApiController]
public class CategoryController([FromServices] ApplicationDBContext dbContext, ILogger<CategoryController> logger) : ControllerBase
{
    // GET: api/categories
    [HttpGet]
    public IActionResult ListCategories()
    {
        logger.LogInformation("fetching and returning all the categories");
        return Ok(dbContext.Categories);
    }

    // GET: api/categories/:id
    [HttpGet("{id:guid}")]
    public IActionResult RetrieveCategory(Guid id)
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
}
