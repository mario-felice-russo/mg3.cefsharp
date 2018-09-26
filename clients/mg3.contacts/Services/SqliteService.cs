using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class SqliteService<T> where T : new()
    {
        public SqliteService(string databaseName)
        {
            DatabaseName = databaseName;
            Create();
        }

        public string DatabaseName { get; set; } = null;
        public T Entity { get; set; } = default(T);
        public List<T> Entities { get; set; } = null;

        public string DatabasePath
        {
            get
            {
                return System.IO.Path.Combine(
                    Environment.CurrentDirectory,
                    DatabaseName + ".db"
                );
            }
        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(DatabasePath, true);
        }

        public CreateTableResult Create()
        {
            CreateTableResult result;

            using (SQLiteConnection connection = GetConnection())
            {
                result = connection.CreateTable<T>();
            }

            return result;
        }

        public int Insert(T item)
        {
            int result = -1;

            using (SQLiteConnection connection = GetConnection())
            {
                result = connection.Insert(item);
            }

            return result;
        }

        public List<T> Read()
        {
            using (SQLiteConnection connection = GetConnection())
            {
                Entities = connection.Table<T>().ToList();
            }

            return Entities;
        }
    }
}
