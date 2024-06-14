using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TasksWebApi.Models;
using TasksWebApi.Services;

namespace TasksWebApi.Tests.Services;

public class CategoryServiceTest
{
    [Fact]
    public void GetAllCategories_should_return_list_of_categories()
    {

        // Arrange
        var loggerMock = new Mock<ILogger<CategoryService>>();
        var mockDBContext = new Mock<ApplicationDBContext>();
        var data = GenerateData();

        MockDBContext(mockDBContext, data);

        var service = new CategoryService(mockDBContext.Object, loggerMock.Object);

        var results = service.GetAllCategories().ToList();

        Assert.Equal(2, results.Count);
        Assert.Equal("Category 1", results[0].Name);
        Assert.Equal("Category 2", results[1].Name);
    }

    private static void MockDBContext(Mock<ApplicationDBContext> mockContext, IQueryable<Category> data)
    {
        var mockSet = new Mock<DbSet<Category>>();
        mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

        mockContext.Setup(c => c.Categories).Returns(mockSet.Object);
    }

    private static IQueryable<Category> GenerateData()
    {
        return new List<Category>()
        {
            new() {
                Id = Guid.NewGuid(),
                Name = "Category 1",
                Description = "Category 1",
                Weight = 50,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
            new() {
                Id = Guid.NewGuid(),
                Name = "Category 2",
                Description = "Category 2",
                Weight = 50,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
        }.AsQueryable();
    }
}
