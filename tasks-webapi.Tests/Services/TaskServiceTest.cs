using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TasksWebApi.Services;
using TasksWebApi.Tests.DataMocks;

namespace TasksWebApi.Tests.Services;

public class TaskServiceTest
{
    private readonly Mock<ILogger<TaskService>> logger = new();

    [Fact]
    public void GetAllTasks_should_return_the_list_of_tasks()
    {
        var mockSet = new Mock<DbSet<Models.Task>>();
        var data = TaskDataMocks.GenerateQueryable(2);
        mockSet.As<IQueryable<Models.Task>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<Models.Task>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<Models.Task>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<Models.Task>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
        var mockContext = new Mock<ApplicationDBContext>();
        mockContext.Setup(p => p.Tasks).Returns(mockSet.Object);

        var service = new TaskService(mockContext.Object, logger.Object);
        var result = service.GetAllTasks().ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal("Task 1", result[0].Name);
        Assert.Equal("Task 2", result[1].Name);
    }

    [Fact]
    public void GetTaskById_should_return_task()
    {
        var mockDBContext = new Mock<ApplicationDBContext>();
        var data = TaskDataMocks.Generate(2);
        var mockItem = data.First();
        mockDBContext.Setup(c => c.Tasks.Find(It.IsAny<Guid>())).Returns(mockItem);

        var service = new TaskService(mockDBContext.Object, logger.Object);
        var result = service.GetTaskById(mockItem.Id);

        Assert.NotNull(result);
        Assert.Equal(result.Id, mockItem.Id);
    }

    [Fact]
    public void GetTaskById_should_return_null_if_does_not_exists()
    {
        var mockDBContext = new Mock<ApplicationDBContext>();
        mockDBContext.Setup(c => c.Tasks.Find(It.IsAny<Guid>()));
        var id = Guid.NewGuid();

        var service = new TaskService(mockDBContext.Object, logger.Object);
        var result = service.GetTaskById(id);

        Assert.Null(result);
    }
}
