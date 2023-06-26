﻿// Ignore Spelling: sql App

using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace HotelAppLibrary.Databases
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public List<T> LoadData<T, U>(string sqlStatement,
                                      U parameters,
                                      string connectionStringName,
                                      bool isStoreProcedure = false)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);
            CommandType commandType = CommandType.Text;
            if (isStoreProcedure)
            {
                commandType = CommandType.StoredProcedure;
            }


            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sqlStatement, parameters, commandType: commandType).ToList();
                return rows;
            }
        }

        public void SaveData<T>(string sqlStatement,
                                T parameters,
                                string connectionStringName,
                                 bool isStoreProcedure = false)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);
            CommandType commandType = CommandType.Text;
            if (isStoreProcedure)
            {
                commandType = CommandType.StoredProcedure;
            }

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(sqlStatement, parameters, commandType: commandType);
            }
        }

    }
}
