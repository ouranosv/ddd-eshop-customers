using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace DataAccess.UnitsOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();

        Task<DbDataReader> ExecuteReaderAsync(string commandText, DbParameter[] commandParameters);

        Task<int> ExecuteNonQueryAsync(string commandText, DbParameter[] commandParameters);

        Task<object> ExecuteScalarAsync(string commandText, DbParameter[] commandParameters);
    }
}
