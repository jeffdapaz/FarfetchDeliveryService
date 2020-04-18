using System.Collections.Generic;
using System.Threading.Tasks;

namespace FarfetchDeliveryServiceGraphRepository.Domain.Interfaces
{
    /// <summary>
    /// Interface for Point Repository
    /// </summary>
    public interface IPointRepository
    {
        /// <summary>
        /// Get all points from the database
        /// </summary>
        /// <returns>Points</returns>
        Task<IEnumerable<string>> Get();

        /// <summary>
        /// Check if a point exists
        /// </summary>
        /// <returns>True if a point already exists</returns>
        Task<bool> CheckIfExists(string name);

        /// <summary>
        /// Add a new point to the database
        /// </summary>
        /// <param name="point">Point's name for the new point</param>
        Task Add(string name);

        /// <summary>
        /// Remove a point from database
        /// </summary>
        /// <param name="name">Point's name thaat will be deleted</param>
        Task Delete(string name);
    }
}
