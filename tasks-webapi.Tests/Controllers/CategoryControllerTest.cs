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
        categoryService.Setup(c => c.GetAllCategories()).Returns(data);

        var controller = new CategoryController(logger.Object, categoryService.Object);
        var result = controller.List();

        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<Category>>(okResult.Value);
        var category = returnValue.First();
        Assert.Equal(2, data.Count());
        Assert.Equal(category.Id, data.First().Id);
    }
}
