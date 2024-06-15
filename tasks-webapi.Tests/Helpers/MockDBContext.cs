using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace TasksWebApi.Tests.Helpers;

static public class MockDBContext
{
    public static void Setup<T>(Mock<ApplicationDBContext> mock, IQueryable<T> data, Expression<Func<ApplicationDBContext, DbSet<T>>> selector) where T : class
    {
        var dbSet = new Mock<DbSet<T>>();
        dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
        dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
        dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
        dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

        dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(i => {
            data.Append(i);
        });

        mock.Setup(selector).Returns(dbSet.Object);
    }
}
