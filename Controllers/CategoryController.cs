using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace TasksWebApi.Controllers;

[Route("api/categories")]
[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
public class CategoryController(ILogger<CategoryController> _logger, Services.ICategoryService _categoryService) : ControllerBase, ICategoryController
{
    // GET: api/categories
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Models.Category>), 200)]
    [SwaggerOperation(Description = "Returns all the available categories")]
    public IActionResult List()
    {
        return Ok(_categoryService.GetAllCategories());
    }

    // GET: api/categories/:id
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Models.Category), 200)]
    [SwaggerOperation(Description = "Returns the information of a single category by its ID")]
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
    [ProducesResponseType(typeof(Models.Category), 200)]
    [SwaggerOperation(Description = "Creates a new category")]
    public IActionResult Create([FromBody] DTOs.CategoryDTO body)
    {
        var item = _categoryService.CreateCategory(body);
        return Ok(item);
    }

    // PUT: api/categories/:id
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Models.Category), 200)]
    [SwaggerOperation(Description = "Updates a category by providing its ID and the data to update")]
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
    [ProducesResponseType(typeof(Models.Category), 200)]
    [SwaggerOperation(Description = "Deletes a category by its ID.")]
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
        return NotFound(new DTOs.ApiMessageDto {
            Message = $"Category with ID '{id}' was not found"
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
