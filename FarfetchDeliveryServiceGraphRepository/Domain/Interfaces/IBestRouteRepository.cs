using FarfetchDeliveryServiceGraphRepository.Entities;
using System.Threading.Tasks;

namespace FarfetchDeliveryServiceGraphRepository.Domain.Interfaces
{
    /// <summary>
    /// Interface for Best Route Repository
    /// </summary>
    public interface IBestRouteRepository
    {
        /// <summary>
        /// Get the best possible route between departure and final destiny points 
        /// </summary>
        /// <param name="pointDepartureName">Departure's name</param>
        /// <param name="pointDestinyName">Destiny's name</param>
        /// <returns>Best possible route between departure and final destiny points</returns>
        Task<BestRoute> Get(string pointDepartureName, string pointDestinyName);
    }
}
