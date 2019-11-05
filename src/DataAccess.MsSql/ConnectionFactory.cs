using System;
using System.Data.SqlClient;

namespace DataAccess.MsSql
{
    public class ConnectionFactory
    {
        private readonly string _connectionString;

        public ConnectionFactory(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException("connectionString", "Connection string cannot be null");

            _connectionString = connectionString;
        }

        public SqlConnection Create()
        {
            var connection = new SqlConnection(_connectionString);

            connection.Open();

            return connection;
        }
    }
}
