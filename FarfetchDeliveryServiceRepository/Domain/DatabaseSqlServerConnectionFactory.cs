using FarfetchDeliveryServiceSqlServerRepository.Domain.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace FarfetchDeliveryServiceSqlServerRepository.Domain
{
    /// <summary>
    /// Factory for connection to the SQL Server database
    /// </summary>
    public class DatabaseSqlServerConnectionFactory : IDatabaseSqlServerConnectionFactory
    {
        private readonly string _connectionString;
        private IDbConnection _connection;

        /// <summary>
        /// Constructor default
        /// </summary>
        /// <param name="connectionString">String database connection</param>
        public DatabaseSqlServerConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Get the database connection
        /// </summary>
        /// <returns>Connection</returns>
        public IDbConnection GetConnection()
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(_connectionString);

                _connection.Open();
            }

            return _connection;
        }
    }
}
