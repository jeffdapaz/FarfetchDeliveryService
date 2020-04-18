using System.Collections.Generic;

namespace FarfetchDeliveryServiceBestRouteApi.Models
{
    /// <summary>
    /// Model for best route result
    /// </summary>
    public class BestRoute
    {
        /// <summary>
        /// Point's departure name
        /// </summary>
        public string PointDepartureName { get; set; }

        /// <summary>
        /// Point's destiny name
        /// </summary>
        public string PointDestinyName { get; set; }

        /// <summary>
        /// Total effort to delevery by this route
        /// </summary>
        public int TotalEffort { get; set; }

        /// <summary>
        /// The complete route path
        /// </summary>
        public List<Path> CompletePath { get; set; }
    }

    /// <summary>
    /// Entity that represents a path
    /// </summary>
    public class Path
    {
        /// <summary>
        /// Point's name
        /// </summary>
        public string PointName { get; set; }

        /// <summary>
        /// Effort to get this point
        /// </summary>
        public int Effort { get; set; }
    }
}
