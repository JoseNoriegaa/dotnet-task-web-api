using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TasksWebApi.Controllers;
using TasksWebApi.Models;
using TasksWebApi.Services;
using TasksWebApi.Tests.DataMocks;

namespace TasksWebApi.Tests.Controllers;

public class CategoryControllerTest
{
    private readonly Mock<ILogger<ICategoryController>> logger = new();

    [Fact]
    public void List_should_return_the_list_of_categories()
    {
        var data = CategoryDataMocks.Generate(2);
        var categoryService = new Mock<ICategoryService>();
        categoryService.Setup(static c => c.GetAllCategories()).Returns(data);

        var controller = new CategoryController(logger.Object, categoryService.Object);
        var result = controller.List();

        categoryService.Verify(c => c.GetAllCategories(), Times.Once);
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<Category>>(okResult.Value);
        var category = returnValue.First();
        Assert.Equal(2, data.Count());
        Assert.Equal(category.Id, data.First().Id);
    }

    [Fact]
    public void Retrieve_should_return_a_category()
    {
        var data = CategoryDataMocks.Generate(1);
        var item = data.First();
        var categoryService = new Mock<ICategoryService>();
        categoryService.Setup(c => c.GetCategoryById(item.Id)).Returns(item);

        var controller = new CategoryController(logger.Object, categoryService.Object);
        var result = controller.Retrieve(item.Id);

        categoryService.Verify(c => c.GetCategoryById(item.Id), Times.Once);
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<Category>(okResult.Value);
        Assert.Equal(returnValue.Id, item.Id);
    }

    [Fact]
    public void Retrieve_should_return_not_found()
    {
        var id = Guid.NewGuid();
        var categoryService = new Mock<ICategoryService>();
        categoryService.Setup(c => c.GetCategoryById(id));

        var controller = new CategoryController(logger.Object, categoryService.Object);
        var result = controller.Retrieve(id);

        Assert.NotNull(result);
        var okResult = Assert.IsType<NotFoundObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<DTOs.ApiMessageDto>(okResult.Value);
        Assert.Equal($"Category with ID '{id}' was not found", returnValue.Message);
        categoryService.Verify(c => c.GetCategoryById(id), Times.Once);
    }

    [Fact]
    public void Create_should_create_an_return_a_category()
    {
        var data = new DTOs.CategoryDTO()
        {
            Name = "New category"
        };
        var item = CategoryDataMocks.Generate(1).First();
        item.Name = data.Name;
        var categoryService = new Mock<ICategoryService>();
        categoryService.Setup(c => c.CreateCategory(data)).Returns(item);

        var controller = new CategoryController(logger.Object, categoryService.Object);
        var result = controller.Create(data);

        categoryService.Verify(c => c.CreateCategory(data), Times.Once);
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<Category>(okResult.Value);
        Assert.StrictEqual(returnValue, item);
    }

    [Fact]
    public void Update_should_update_and_return_the_updated_category()
    {
        var data = new DTOs.CategoryDTO() { Name = "Updated" };
        var item = CategoryDataMocks.Generate(1).First();
        var categoryService = new Mock<ICategoryService>();
        categoryService.Setup(c => c.GetCategoryById(item.Id)).Returns(item);
        categoryService.Setup(c => c.UpdateCategory(item, data));

        var controller = new CategoryController(logger.Object, categoryService.Object);
        var result = controller.Update(item.Id, data);

        categoryService.Verify(c => c.UpdateCategory(item, data), Times.Once);
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<Category>(okResult.Value);
        Assert.StrictEqual(returnValue, item);
    }

    [Fact]
    public void Update_should_not_found()
    {
        var id = Guid.NewGuid();
        var data = new DTOs.CategoryDTO() { Name = "Updated" };
        var categoryService = new Mock<ICategoryService>();
        categoryService.Setup(c => c.GetCategoryById(id));

        var controller = new CategoryController(logger.Object, categoryService.Object);
        var result = controller.Update(id, data);

        categoryService.Verify(c => c.UpdateCategory(It.IsAny<Category>(), It.IsAny<DTOs.CategoryDTO>()), Times.Never);
        Assert.NotNull(result);
        var okResult = Assert.IsType<NotFoundObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<DTOs.ApiMessageDto>(okResult.Value);
        Assert.Equal($"Category with ID '{id}' was not found", returnValue.Message);
    }

    [Fact]
    public void Delete_should_delete_and_return_a_category()
    {
        var item = CategoryDataMocks.Generate(1).First();
        var categoryService = new Mock<ICategoryService>();
        categoryService.Setup(c => c.GetCategoryById(item.Id)).Returns(item);
        categoryService.Setup(c => c.DeleteCategory(item));

        var controller = new CategoryController(logger.Object, categoryService.Object);
        var result = controller.Delete(item.Id);

        categoryService.Verify(c => c.DeleteCategory(item), Times.Once);
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<Category>(okResult.Value);
        Assert.StrictEqual(returnValue, item);
    }

    [Fact]
    public void Delete_should_return_not_found()
    {
        var id = Guid.NewGuid();
        var categoryService = new Mock<ICategoryService>();
        categoryService.Setup(c => c.GetCategoryById(id));

        var controller = new CategoryController(logger.Object, categoryService.Object);
        var result = controller.Delete(id);

        categoryService.Verify(c => c.DeleteCategory(It.IsAny<Category>()), Times.Never);
        Assert.NotNull(result);
        var okResult = Assert.IsType<NotFoundObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<DTOs.ApiMessageDto>(okResult.Value);
        Assert.Equal($"Category with ID '{id}' was not found", returnValue.Message);
    }

    [Fact]
    public void Delete_should_return_conflict_if_there_are_task_associated_with_the_category()
    {
        var item = CategoryDataMocks.Generate(1).First();
        var categoryService = new Mock<ICategoryService>();
        categoryService.Setup(c => c.GetCategoryById(item.Id)).Returns(item);
        categoryService.Setup(c => c.CountRelatedTasks(item.Id)).Returns(2);

        var controller = new CategoryController(logger.Object, categoryService.Object);
        var result = controller.Delete(item.Id);

        categoryService.Verify(c => c.DeleteCategory(It.IsAny<Category>()), Times.Never);
        Assert.NotNull(result);
        var okResult = Assert.IsType<ConflictObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<DTOs.ApiMessageDto>(okResult.Value);
        Assert.Equal("Cannot delete a category with associated tasks", returnValue.Message);
    }
}
