using System.Linq.Expressions;

namespace MyHostAPI.Data.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
    }
}