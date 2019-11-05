using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DataAccess.UnitsOfWork;

namespace DataAccess.MsSql.UnitsOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private SqlTransaction _transaction;
        private SqlConnection _connection;

        public UnitOfWork(ConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.Create();
            _transaction = _connection.BeginTransaction();
        }

        public void SaveChanges()
        {
            _transaction.Commit();
            _transaction = _connection.BeginTransaction();
        }

        private DbCommand CreateCommand()
        {
            var command = _connection.CreateCommand();
            command.Transaction = _transaction;

            return command;
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction = null;
            }

            if (_connection != null)
            {
                _connection.Close();
                _connection = null;
            }
        }

        public async Task<DbDataReader> ExecuteReaderAsync(string commandText, DbParameter[] commandParameters)
        {
            DbDataReader reader;

            using (var command = CreateCommand())
            {
                command.CommandText = commandText;
                command.Parameters.AddRange(commandParameters);

                reader = await command.ExecuteReaderAsync();
            }

            return reader;
        }

        public async Task<int> ExecuteNonQueryAsync(string commandText, DbParameter[] commandParameters)
        {
            int affectedRows;

            using (var command = CreateCommand())
            {
                command.CommandText = commandText;
                command.Parameters.AddRange(commandParameters);

                affectedRows = await command.ExecuteNonQueryAsync();
            }

            return affectedRows;
        }

        public async Task<object> ExecuteScalarAsync(string commandText, DbParameter[] commandParameters)
        {
            object result;

            using (var command = CreateCommand())
            {
                command.CommandText = commandText;
                command.Parameters.AddRange(commandParameters);

                result = await command.ExecuteScalarAsync();
            }

            return result;
        }
    }
}
