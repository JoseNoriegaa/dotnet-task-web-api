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

        var mockSet = new Mock<DbSet<Category>>();
        mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

        mockDBContext.Setup(c => c.Categories).Returns(mockSet.Object);

        // Act
        var service = new CategoryService(mockDBContext.Object, loggerMock.Object);

        var results = service.GetAllCategories().ToList();

        // Assert
        Assert.Equal(2, results.Count);
        Assert.Equal("Category 1", results[0].Name);
        Assert.Equal("Category 2", results[1].Name);
    }

    [Fact]
    public void GetCategoryById_should_return_existing_category()
    {
        var loggerMock = new Mock<ILogger<CategoryService>>();
        var mockDBContext = new Mock<ApplicationDBContext>();
        var data = GenerateData();
        var mockItem = data.First();

        mockDBContext.Setup(c => c.Categories.Find(It.IsAny<Guid>())).Returns(mockItem);

        // Act
        var service = new CategoryService(mockDBContext.Object, loggerMock.Object);

        var result = service.GetCategoryById(mockItem.Id);

        Assert.NotNull(result);
        Assert.Equal(result.Id, mockItem.Id);
    }

    [Fact]
    public void GetCategoryById_given_an_invalid_id_should_return_null()
    {
        var loggerMock = new Mock<ILogger<CategoryService>>();
        var mockDBContext = new Mock<ApplicationDBContext>();

        mockDBContext.Setup(static c => c.Categories.Find(It.IsAny<Guid>()));

        // Act
        var service = new CategoryService(mockDBContext.Object, loggerMock.Object);

        var id = Guid.NewGuid();
        var result = service.GetCategoryById(id);

        Assert.Null(result);
    }

    private static IQueryable<Category> GenerateData()
    {
        return new List<Category>()
        {
            new() {
                Id = Guid.Parse("e9d2de54-d048-42fd-8715-251875766097"),
                Name = "Category 1",
                Description = "Category 1",
                Weight = 50,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
            new() {
                Id = Guid.Parse("e9d2de54-d048-42fd-8715-251875766097"),
                Name = "Category 2",
                Description = "Category 2",
                Weight = 50,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
        }.AsQueryable();
    }
}
