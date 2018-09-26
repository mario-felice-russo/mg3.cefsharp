using System;
using System.Linq.Expressions;

namespace mg3.data
{
    public interface IFilter<T> where T : class, new()
    {
        string EntityName { get; }
        string Query { get; }

        void Add(Expression<Func<T, object>> memberExpression, object memberValue);
    }
}
