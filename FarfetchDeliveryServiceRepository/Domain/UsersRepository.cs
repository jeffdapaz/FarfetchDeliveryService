using Dapper;
using FarfetchDeliveryServiceRepository.Entity;
using FarfetchDeliveryServiceSqlServerRepository.Domain.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FarfetchDeliveryServiceRepository.Domain
{
    /// <summary>
    /// Repository for users entity
    /// </summary>
    public class UsersRepository : IUsersRepository
    {
        private readonly IDatabaseSqlServerConnectionFactory _connectionFactory;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="databaseConnectionFactory">Factory for connection to the database</param>
        public UsersRepository(IDatabaseSqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Get a user by login
        /// </summary>
        /// <param name="login">User's login</param>
        /// <returns>User</returns>
        public async Task<Users> GetByLogin(string login)
        {
            string query = "SELECT TOP 1 * FROM Users WITH(NOLOCK) WHERE Login = @Login";

            using (IDbConnection connection = _connectionFactory.GetConnection())
            {
                IEnumerable<Users> result = await connection.QueryAsync<Users>(query, new { Login = login });

                return result.FirstOrDefault();
            }
        }
    }
}
