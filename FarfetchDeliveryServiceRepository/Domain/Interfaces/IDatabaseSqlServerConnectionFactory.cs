using System.Data;

namespace FarfetchDeliveryServiceSqlServerRepository.Domain.Interfaces
{
    /// <summary>
    /// Interface factory for connection to the SQL Server database
    /// </summary>
    public interface IDatabaseSqlServerConnectionFactory
    {
        /// <summary>
        /// Get the database connection
        /// </summary>
        /// <returns>Connection</returns>
        IDbConnection GetConnection();
    }
}
