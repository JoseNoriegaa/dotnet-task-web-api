using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TasksWebApi.Services;
using TasksWebApi.Tests.DataMocks;

namespace TasksWebApi.Tests.Services;

public class TaskServiceTest
{
    [Fact]
    public void GetAllTasks()
    {
        var logger = new Mock<ILogger<TaskService>>();
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
}
