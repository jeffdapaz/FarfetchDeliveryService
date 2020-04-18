using Neo4jClient;

namespace FarfetchDeliveryServiceGraphRepository.Domain.Interfaces
{
    /// <summary>
    /// Interface factory for connection to the database
    /// </summary>
    public interface IDatabaseGraphConnectionFactory
    {
        /// <summary>
        /// Get the database connection
        /// </summary>
        /// <returns>Connection</returns>
        IGraphClient GetConnection();
    }
}
