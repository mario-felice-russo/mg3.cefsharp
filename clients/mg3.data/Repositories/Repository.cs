using Databases;
using Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : ICloneable, new()
    {
        public Repository()
        {
            Connection = DatabaseFactory.GetDb().Result;
        }

        public Repository(SQLiteAsyncConnection connection)
        {
            Connection = connection;
        }

        public Repository(DatabaseEnum db)
        {
            Connection = DatabaseFactory.GetDb(db).Result;
        }

        public T Entity { get; set; }
        public SQLiteAsyncConnection Connection { get; set; } = null;

        private async Task<bool> Close(SQLiteAsyncConnection connection)
        {
            bool result = false;

            if (connection != null)
            {
                await Task.Factory.StartNew(() =>
                {
                    connection.GetConnection().Close();
                    connection.GetConnection().Dispose();
                    connection = null;

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    result = true;
                });
            }

            return result;
        }

        public async Task<bool> CheckIdExist(int id)
        {
            int count = 0;

            try
            {
                string entityName = typeof(T).Name;
                count = Convert.ToInt32(await Connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM " + entityName + " WHERE Id=" + id));
            }
            catch (Exception e)
            {
                while (e != null)
                {
                    Debug.WriteLine(e.Message);
                    e = e.InnerException;
                }
            }

            return count > 0;
        }

        /// <summary>
        /// Inserisce una entità nel datbase e nella primaryKey sarà inserita la nuova chiave aggiunta.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<SqliteResult<T>> InsertAsync(T entity)
        {
            Entity = entity;
            SqliteResult<T> result = new SqliteResult<T>();

            try
            {
                if (VerifyEntity())
                {
                    result.NrRecords = await Connection.InsertAsync(entity);
                    result.Entity = entity;
                }
            }
            catch (SQLiteException se)
            {
                result.Error = se;
            }

            return result;
        }

        public async Task<SqliteResult<T>> UpdateAsync(T entity)
        {
            Entity = entity;
            SqliteResult<T> result = new SqliteResult<T>();

            try
            {
                if (VerifyEntity())
                {
                    result.NrRecords = await Connection.UpdateAsync(entity);
                    result.Entity = (T)entity.Clone();
                }
            }
            catch (SQLiteException se)
            {
                result.Error = se;
            }

            return result;
        }

        public async Task<SqliteResult<T>> UpsertAsync(T entity)
        {
            Entity = entity;
            SqliteResult<T> result = new SqliteResult<T>();

            try
            {
                if (VerifyEntity())
                {
                    result.NrRecords = await Connection.InsertOrReplaceAsync(entity);
                    result.Entity = (T)entity.Clone();
                }
            }
            catch (SQLiteException se)
            {
                result.Error = se;
            }

            return result;
        }

        public async Task<SqliteResult<T>> DeleteAsync(T entity)
        {
            Entity = entity;
            SqliteResult<T> result = new SqliteResult<T>();

            try
            {
                if (VerifyEntity())
                {
                    result.NrRecords = await Connection.DeleteAsync(entity);
                    result.Entity = (T)entity.Clone();
                }
            }
            catch (SQLiteException se)
            {
                result.Error = se;
            }

            return result;
        }

        public async Task<SqliteResult<T>> SelectByIdAsync(int id)
        {
            SqliteResult<T> result = new SqliteResult<T>();

            try
            {
                if (await CheckIdExist(id))
                {
                    result.Entity = await Connection.GetAsync<T>(id);
                    result.NrRecords = 1;
                }
            }
            catch (SQLiteException se)
            {
                result.Error = se;
            }

            return result;
        }

        public async Task<SqliteResult<T>> SelectAllAsync()
        {
            SqliteResult<T> result = new SqliteResult<T>();

            try
            {
                if (Connection != null)
                {
                    AsyncTableQuery<T> table = Connection.Table<T>();
                    int nr = await table.CountAsync();

                    if (nr > 0)
                    {
                        result.Entities = new List<T>(await table.ToListAsync());
                        result.NrRecords = result.Entities.Count;
                    }
                }
            }
            catch (SQLiteException se)
            {
                result.Error = se;
            }

            return result;
        }

        public async Task<SqliteResult<T>> SelectAsync(string query, params object[] parameters)
        {
            SqliteResult<T> result = new SqliteResult<T>();

            try
            {
                result.Entities = new List<T>(await Connection.QueryAsync<T>(query, parameters));
                result.NrRecords = result.Entities.Count;
            }
            catch (SQLiteException se)
            {
                result.Error = se;
            }

            return result;
        }

        public async Task<SqliteResult<T>> DropAsync()
        {
            SqliteResult<T> result = new SqliteResult<T>();

            try
            {
                result.NrRecords = await Connection.DropTableAsync<T>();
            }
            catch (SQLiteException se)
            {
                result.Error = se;
            }

            return result;
        }

        public abstract bool VerifyExistencePrimaryKey();
        public abstract bool VerifyEntity();
    }
}
