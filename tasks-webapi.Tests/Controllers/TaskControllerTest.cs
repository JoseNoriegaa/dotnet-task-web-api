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
        var response = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<Models.Task>>(response.Value);
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
        var response = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<Models.Task>(response.Value);
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
        var response = Assert.IsType<NotFoundObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<DTOs.ApiMessageDto>(response.Value);
        Assert.Equal($"Task with ID '{item.Id}' was not found", returnValue.Message);
    }

    [Fact]
    public void CreateTask_should_create_and_return_a_task()
    {
        var item = TaskDataMocks.Generate(1).First();
        var taskService = new Mock<ITaskService>();
        var data = new DTOs.TaskDTO {
            Name = item.Name,
            Description = item.Description,
            CategoryId = item.CategoryId,
            Priority = item.Priority,
        };
        categoryService.Setup(c => c.Exists(data.CategoryId)).Returns(true);
        taskService.Setup(c => c.CreateTask(data)).Returns(item);

        var controller = new TaskController(logger.Object, taskService.Object, categoryService.Object);
        var result = controller.Create(data);

        taskService.Verify(c => c.CreateTask(data), Times.Once);
        Assert.NotNull(result);
        var response = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<Models.Task>(response.Value);
        Assert.Equal(returnValue.Name, data.Name);
        Assert.Equal(returnValue.Description, data.Description);
        Assert.Equal(returnValue.CategoryId, data.CategoryId);
        Assert.Equal(returnValue.Priority, data.Priority);
    }

    [Fact]
    public void CreateTask_should_return_400_response_if_category_not_exists()
    {
        var item = TaskDataMocks.Generate(1).First();
        var taskService = new Mock<ITaskService>();
        var data = new DTOs.TaskDTO {
            Name = item.Name,
            Description = item.Description,
            CategoryId = item.CategoryId,
            Priority = item.Priority,
        };
        categoryService.Setup(c => c.Exists(It.IsAny<Guid>())).Returns(false);

        var controller = new TaskController(logger.Object, taskService.Object, categoryService.Object);
        var result = controller.Create(data);

        taskService.Verify(c => c.CreateTask(It.IsAny<DTOs.TaskDTO>()), Times.Never);
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }
}
