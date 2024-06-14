using TasksWebApi.Models;

namespace TasksWebApi.Tests.DataMocks;

static class TaskDataMocks
{
    public static IEnumerable<Models.Task> Generate(int quantity = 1)
    {

        var categories = CategoryDataMocks.Generate().ToList();
        var timestamp = DateTime.UtcNow;
        var list = new List<Models.Task>();

        for (int i = 0; i < quantity; i++)
        {
            list.Add(new() {
                Id = Guid.NewGuid(),
                Name = $"Task {i + 1}",
                Description = null,
                CategoryId = categories[0].Id,
                Priority = Priority.LOW,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            });
        }

        return list;
    }

    public static IQueryable<Models.Task> GenerateQueryable(int quantity = 1)
    {
        return Generate(quantity).AsQueryable();
    }
}
