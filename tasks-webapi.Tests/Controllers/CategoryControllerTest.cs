using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TasksWebApi.Controllers;
using TasksWebApi.Models;
using TasksWebApi.Services;
using TasksWebApi.Tests.DataMocks;
using TasksWebApi.Tests.Helpers;

namespace TasksWebApi.Tests.Controllers;

public class CategoryControllerTest
{
    private readonly Mock<ILogger<ICategoryController>> logger = new();

    [Fact]
    public void List_should_return_the_list_of_tasks()
    {
        var data = CategoryDataMocks.Generate(2);
        var categoryService = new Mock<ICategoryService>();
        categoryService.Setup(static c => c.GetAllCategories()).Returns(data);

        var controller = new CategoryController(logger.Object, categoryService.Object);
        var result = controller.List();

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

        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<Category>(okResult.Value);
        Assert.StrictEqual(returnValue, item);
    }
}
