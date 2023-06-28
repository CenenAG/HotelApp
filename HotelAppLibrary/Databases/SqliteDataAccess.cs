// Ignore Spelling: sql App sqlite

using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace HotelAppLibrary.Databases
{
    public class SqliteDataAccess : ISqliteDataAccess
    {
        private readonly IConfiguration _config;

        public SqliteDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public List<T> LoadData<T, U>(string sqliteStatement,
                                      U parameters,
                                      string connectionStringName)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sqliteStatement, parameters).ToList();
                return rows;
            }
        }

        public void SaveData<T>(string sqliteStatement,
                                T parameters,
                                string connectionStringName)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);


            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Execute(sqliteStatement, parameters);
            }
        }

    }
}
