using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TasksWebApi.Controllers;
using TasksWebApi.Models;
using TasksWebApi.Services;
using TasksWebApi.Tests.DataMocks;

namespace TasksWebApi.Tests.Controllers;

public class TaskControllerTest
{
    private readonly Mock<ILogger<ITaskController>> logger = new();
    private readonly Mock<ICategoryService> categoryService = new();

    [Fact]
    public void List_should_return_the_list_of_tasks()
    {
        var data = TaskDataMocks.Generate(2);
        var taskService = new Mock<ITaskService>();
        taskService.Setup(static c => c.GetAllTasks()).Returns(data);

        var controller = new TaskController(logger.Object, taskService.Object, categoryService.Object);
        var result = controller.List();

        taskService.Verify(c => c.GetAllTasks(), Times.Once);
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<Models.Task>>(okResult.Value);
        var task = returnValue.First();
        Assert.Equal(2, data.Count());
        Assert.Equal(task.Id, data.First().Id);
    }

    [Fact]
    public void Retrieve_should_return_task()
    {
        var data = TaskDataMocks.Generate(1);
        var item = data.First();
        var taskService = new Mock<ITaskService>();
        taskService.Setup(c => c.GetTaskById(item.Id)).Returns(item);

        var controller = new TaskController(logger.Object, taskService.Object, categoryService.Object);
        var result = controller.Retrieve(item.Id);

        taskService.Verify(c => c.GetTaskById(item.Id), Times.Once);
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<Models.Task>(okResult.Value);
        Assert.Equal(returnValue.Id, item.Id);
    }

    [Fact]
    public void Retrieve_should_return_not_found()
    {
        var data = TaskDataMocks.Generate(1);
        var item = data.First();
        var taskService = new Mock<ITaskService>();
        taskService.Setup(c => c.GetTaskById(item.Id));

        var controller = new TaskController(logger.Object, taskService.Object, categoryService.Object);
        var result = controller.Retrieve(item.Id);

        taskService.Verify(c => c.GetTaskById(item.Id), Times.Once);
        Assert.NotNull(result);
        var okResult = Assert.IsType<NotFoundObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<DTOs.ApiMessageDto>(okResult.Value);
        Assert.Equal($"Task with ID '{item.Id}' was not found", returnValue.Message);
    }
}
