using Databases;
using Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : ICloneable, new()
    {
        public Repository()
        {
            Task.Run(async () => _connection = await DatabaseFactory.GetDb());
        }

        public Repository(SQLiteAsyncConnection connection)
        {
            Connection = connection;
        }

        public Repository(DatabaseEnum db)
        {
            Task.Run(async () => _connection = await DatabaseFactory.GetDb(db));
        }

        public T Entity { get; set; }
        public SQLiteAsyncConnection Connection
        {
            get
            {
                return _connection == null ? DatabaseFactory.GetDb().Result : _connection;
            }
            set
            {
                _connection = value;
            }
        }
        private SQLiteAsyncConnection _connection = null;

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
                    result.Entity = (T) entity.Clone();
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
                result.Entity = await Connection.GetAsync<T>(id);
                result.NrRecords = 1;
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
                result.Entities = new List<T>(await Connection.Table<T>().ToListAsync());
                result.NrRecords = result.Entities.Count;
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
