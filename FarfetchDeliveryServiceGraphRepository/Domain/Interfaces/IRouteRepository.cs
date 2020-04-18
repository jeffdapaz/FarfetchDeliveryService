using FarfetchDeliveryServiceGraphRepository.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FarfetchDeliveryServiceGraphRepository.Domain.Interfaces
{
    /// <summary>
    /// Interface for Route Repository
    /// </summary>
    public interface IRouteRepository
    {
        /// <summary>
        /// Get a route between two points
        /// </summary>
        /// <param name="pointDepartureName">Departure's name</param>
        /// <param name="pointDestinyName">Destiny's name</param>
        /// <returns>Route</returns>
        Task<Route> Get(string pointDepartureName, string pointDestinyName);

        /// <summary>
        /// Return a route list that have a point as departure
        /// </summary>
        /// <param name="pointDepartureName">Departure's name</param>
        /// <returns>Route list that have a point as departure</returns>
        Task<IEnumerable<Route>> GetByDeparture(string pointDepartureName);

        /// <summary>
        /// Return a route list that have a point as destiny
        /// </summary>
        /// <param name="pointDestinyName">Destiny's name</param>
        /// <returns>Route list that have a point as destiny</returns>
        Task<IEnumerable<Route>> GetByDestiny(string pointDestinyName);

        /// <summary>
        /// Add a new route to the database
        /// </summary>
        /// <param name="route">Route that will be added</param>
        Task Add(Route route);

        /// <summary>
        /// Update a existent route
        /// </summary>
        /// <param name="route">Route that will be updated</param>
        Task Update(Route route);

        /// <summary>
        /// Remove a route between two points from the database
        /// </summary>
        /// <param name="pointDepartureName">Departure's name</param>
        /// <param name="pointDestinyName">Destiny's name</param>
        Task Delete(string pointDepartureName, string pointDestinyName);
    }
}
