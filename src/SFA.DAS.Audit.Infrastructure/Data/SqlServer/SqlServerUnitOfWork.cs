using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace SFA.DAS.Audit.Infrastructure.Data.SqlServer
{
    public class SqlServerUnitOfWork : IDisposable
    {
        private readonly SqlConnection _connection;
        private readonly SqlTransaction _transaction;
        private bool _committed = false;

        public SqlServerUnitOfWork(SqlConnection connection, IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            _connection = connection;
            _transaction = connection.BeginTransaction(isolationLevel);
        }

        public async Task ExecuteAsync(string command, object param = null)
        {
            await _connection.ExecuteAsync(command, param, _transaction);
        }
        public async Task<T[]> QueryAsync<T>(string command, object param = null)
        {
            return (await _connection.QueryAsync<T>(command, param, _transaction)).ToArray();
        }
        public async Task<T> QuerySingleAsync<T>(string command, object param = null)
        {
            return (await QueryAsync<T>(command, param)).SingleOrDefault();
        }

        //Handle calling these twice
        public void CommitChanges()
        {
            _committed = true;
            _transaction.Commit();
        }
        //Handle calling these twice
        public void RollbackChanges()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            if (!_committed)
            {
                RollbackChanges();
            }

            _transaction?.Dispose();
            _connection?.Dispose();
        }
    }
}
