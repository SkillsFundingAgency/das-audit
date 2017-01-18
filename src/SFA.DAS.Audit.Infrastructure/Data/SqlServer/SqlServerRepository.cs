using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Azure;

namespace SFA.DAS.Audit.Infrastructure.Data.SqlServer
{
    public abstract class SqlServerRepository
    {
        private readonly string _connectionString;

        protected SqlServerRepository(string connectionStringKey)
        {
            _connectionString = CloudConfigurationManager.GetSetting(connectionStringKey);
        }

        protected async Task<SqlConnection> GetOpenConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            try
            {
                await connection.OpenAsync();
                return connection;
            }
            catch
            {
                connection.Dispose();
                throw;
            }
        }
        protected async Task<SqlServerUnitOfWork> StartUnitOfWork(SqlConnection connection = null)
        {
            if (connection == null)
            {
                connection = await GetOpenConnectionAsync();
            }
            return new SqlServerUnitOfWork(connection);
        }

        protected async Task<T[]> QueryAsync<T>(string command, object param = null)
        {
            using (var connection = await GetOpenConnectionAsync())
            {
                return await QueryAsync<T>(connection, command, param);
            }
        }
        protected async Task<T[]> QueryAsync<T>(SqlConnection connection, string command, object param = null)
        {
            return (await connection.QueryAsync<T>(command, param)).ToArray();
        }

        protected async Task<T> QuerySingleAsync<T>(string command, object param = null)
        {
            using (var connection = await GetOpenConnectionAsync())
            {
                return await QuerySingleAsync<T>(connection, command, param);
            }
        }
        protected async Task<T> QuerySingleAsync<T>(SqlConnection connection, string command, object param = null)
        {
            return (await connection.QueryAsync<T>(command, param)).SingleOrDefault();
        }
    }
}
