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
        var id = Guid.NewGuid();

        var service = new CategoryService(mockDBContext.Object, loggerMock.Object);
        var result = service.GetCategoryById(id);

        Assert.Null(result);
    }

    [Fact]
    public void CreateCategory_should_create_and_return_a_category()
    {
        var loggerMock = new Mock<ILogger<CategoryService>>();
        var mockDBContext = new Mock<ApplicationDBContext>();

        DTOs.CategoryDTO itemDto = new() {
            Name = "New Category",
            Description = "Description",
        };

        var service = new CategoryService(mockDBContext.Object, loggerMock.Object);
        var newItem = service.CreateCategory(itemDto);

        Assert.NotNull(newItem);
        Assert.Equal(itemDto.Name, newItem.Name);
        Assert.Equal(itemDto.Description, newItem.Description);

        mockDBContext.Verify(
            c => c.Add(newItem),
            Times.Once
        );

        mockDBContext.Verify(
            static c => c.SaveChanges(),
            Times.Once
        );
    }

    [Fact]
    public void CreateCategory_should_add_empty_string_to_description_when_not_provided()
    {
        var loggerMock = new Mock<ILogger<CategoryService>>();
        var mockDBContext = new Mock<ApplicationDBContext>();

        DTOs.CategoryDTO itemDto = new() {
            Name = "New Category",
        };

        var service = new CategoryService(mockDBContext.Object, loggerMock.Object);
        var newItem = service.CreateCategory(itemDto);

        Assert.NotNull(newItem);
        Assert.Equal(itemDto.Name, newItem.Name);
        Assert.Equal("", newItem.Description);
    }

    [Fact]
    public void UpdateCategory_should_update_a_category_with_the_provided_data()
    {
        var loggerMock = new Mock<ILogger<CategoryService>>();
        var mockDBContext = new Mock<ApplicationDBContext>();
        var timestamp = DateTime.UtcNow;
        Category category = new() {
            Id = Guid.Parse("e9d2de54-d048-42fd-8715-251875766097"),
            Name = "Category 1",
            Description = "Category 1",
            Weight = 50,
            CreatedAt = timestamp,
            UpdatedAt = timestamp,
        };
        DTOs.CategoryDTO data = new () {
            Name = "Walk the dog",
            Description = "Take the dog for a walk"
        };

        var service = new CategoryService(mockDBContext.Object, loggerMock.Object);
        service.UpdateCategory(category, data);

        Assert.Equal(category.Name, data.Name);
        Assert.Equal(category.Description, data.Description);
        Assert.NotEqual(category.CreatedAt, category.UpdatedAt);
        mockDBContext.Verify(
            static c => c.SaveChanges(),
            Times.Once
        );
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
