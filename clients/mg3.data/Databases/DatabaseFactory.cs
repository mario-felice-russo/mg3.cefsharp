using Models;
using SQLite;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Databases
{
    public enum DatabaseEnum { none, contacts }

    public static class DatabaseFactory
    {
        public static async Task<SQLiteAsyncConnection> GetDb(DatabaseEnum? db = DatabaseEnum.contacts)
        {
            SQLiteAsyncConnection Connection = null;
            string dbname = null;

            switch (db)
            {
                case DatabaseEnum.none:
                    dbname = null;
                    break;
                case DatabaseEnum.contacts:
                    dbname = "contacts";
                    break;
                default:
                    dbname = "contacts";
                    break;
            }

            string dbPath = Environment.CurrentDirectory + string.Format(@"\Databases\{0}.db", dbname);

            if (File.Exists(dbPath))
            {
                Connection = new SQLiteAsyncConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);

                if (Connection != null)
                    await Connection.CreateTableAsync<Contact>();
            }

            Debug.WriteLine(Connection.DatabasePath);

            return Connection;
        }
    }
}
