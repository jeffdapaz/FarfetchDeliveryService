using FarfetchDeliveryServiceGraphRepository.Domain.Interfaces;
using Neo4jClient;
using System;

namespace FarfetchDeliveryServiceGraphRepository.Domain
{
    /// <summary>
    /// Factory for connection to the database
    /// </summary>
    public class DatabaseGraphConnectionFactory : IDatabaseGraphConnectionFactory
    {
        private readonly string _connectionUrl;
        private readonly string _userName;
        private readonly string _password;
        private GraphClient _graphClient;

        /// <summary>
        /// Constructor default
        /// </summary>
        /// <param name="connectionUrl">Database connection URL</param>
        /// <param name="userName">Database user name</param>
        /// <param name="password">Database password</param>
        public DatabaseGraphConnectionFactory(string connectionUrl, string userName, string password)
        {
            _connectionUrl = connectionUrl;
            _userName = userName;
            _password = password;
        }

        /// <summary>
        /// Get the database connection
        /// </summary>
        /// <returns>Connection</returns>
        public IGraphClient GetConnection()
        {
            if (_graphClient == null)
            {
                _graphClient = new GraphClient(new Uri(_connectionUrl), _userName, _password);

                _graphClient.Connect();
            }

            return _graphClient;
        }
    }
}
