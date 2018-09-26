using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IRepository<T> where T : new()
    {
        Task<SqliteResult<T>> InsertAsync(T entity);
        Task<SqliteResult<T>> UpdateAsync(T entity);
        Task<SqliteResult<T>> UpsertAsync(T entity);
        Task<SqliteResult<T>> DeleteAsync(T entity);
        Task<SqliteResult<T>> SelectByIdAsync(int id);
        Task<SqliteResult<T>> SelectAllAsync();
        Task<SqliteResult<T>> SelectAsync(string query, params object[] parameters);
    }
}
