using TasksWebApi.Models;

namespace TasksWebApi.Tests.DataMocks;

static class CategoryDataMocks
{
    public static IEnumerable<Category> Generate(int quantity = 1)
    {
        var timestamp = DateTime.UtcNow;

        var list = new List<Category>();

        for (int i = 0; i < quantity; i++)
        {
            list.Add(new() {
                Id = Guid.NewGuid(),
                Name = $"Category {i + 1}",
                Description = $"Category {i + 1}",
                Weight = 50,
                CreatedAt = timestamp,
                UpdatedAt = timestamp,
            });
        }

        return list;
    }

    public static IQueryable<Category> GenerateQueryable(int quantity = 1)
    {
        return CategoryDataMocks.Generate(quantity).AsQueryable();
    }
}
