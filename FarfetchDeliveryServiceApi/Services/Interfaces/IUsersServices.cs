using FarfetchDeliveryServiceApi.Models;

namespace FarfetchDeliveryServiceApi.Services.Interfaces
{
    /// <summary>
    /// Interface for the class responsible to manage users
    /// </summary>
    public interface IUsersServices
    {
        /// <summary>
        /// Validate a user autentication and return a token to use in requestes
        /// </summary>
        /// <param name="user">User's data</param>
        /// <returns>Token</returns>
        string Authenticate(User user);
    }
}
