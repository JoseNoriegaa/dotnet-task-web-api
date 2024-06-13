using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkPracticeApp.Controllers;

[Route("api/categories")]
[ApiController]
public class CategoryController(ILogger<CategoryController> _logger, Services.ICategoryService _categoryService) : ControllerBase, ICategoryController
{
    // GET: api/categories
    [HttpGet]
    public IActionResult List()
    {
        return Ok(_categoryService.GetAllCategories());
    }

    // GET: api/categories/:id
    [HttpGet("{id:guid}")]
    public IActionResult Retrieve(Guid id)
    {
        var item = _categoryService.GetCategoryById(id);

        if (item == null) {
            return this.CategoryNotFoundResponse(id);
        }

        return Ok(item);
    }

    // POST: api/categories
    [HttpPost]
    public IActionResult Create([FromBody] DTOs.CategoryDTO body)
    {
        var item = _categoryService.CreateCategory(body);
        return Ok(item);
    }

    // PUT: api/categories/:id
    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, DTOs.CategoryDTO body)
    {
        var item = _categoryService.GetCategoryById(id);
        if (item == null) {
            return this.CategoryNotFoundResponse(id);
        }

        _categoryService.UpdateCategory(item, body);

        return Ok(item);
    }

    // DELETE: api/categories/:id
    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var item = _categoryService.GetCategoryById(id);
        if (item == null) {
            return this.CategoryNotFoundResponse(id);
        }

        var count = _categoryService.CountRelatedTasks(id);
        if (count != 0)
        {
            var msg = "Cannot delete a category with associated tasks.";
            _logger.LogWarning(msg);
            return Conflict(new {
                Message = msg,
            });
        }

        _categoryService.DeleteCategory(item);

        return Ok(item);
    }

    private NotFoundObjectResult CategoryNotFoundResponse(Guid id)
    {
        _logger.LogWarning("Category was not found: {Id}", id);
        return NotFound(new {
            Message = $"Category with ID '{id}' was not found",
        });
    }
}

public interface ICategoryController
{
    public IActionResult List();
    public IActionResult Retrieve(Guid id);
    public IActionResult Create(DTOs.CategoryDTO body);
    public IActionResult Update(Guid id, DTOs.CategoryDTO body);
    public IActionResult Delete(Guid id);
}
