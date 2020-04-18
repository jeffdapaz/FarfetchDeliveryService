using FarfetchDeliveryServiceRepository.Entity;
using System.Threading.Tasks;

namespace FarfetchDeliveryServiceSqlServerRepository.Domain.Interfaces
{
    /// <summary>
    /// Interface for Users Repository
    /// </summary>
    public interface IUsersRepository
    {
        /// <summary>
        /// Get a user by login
        /// </summary>
        /// <param name="login">User's login</param>
        /// <returns>User</returns>
        Task<Users> GetByLogin(string login);
    }
}
