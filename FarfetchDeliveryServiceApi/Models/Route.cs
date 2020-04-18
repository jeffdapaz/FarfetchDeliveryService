using System.ComponentModel.DataAnnotations;

namespace FarfetchDeliveryServiceApi.Models
{
    /// <summary>
    /// Model that represents the route between a departure and destiny points
    /// </summary>
    public class Route
    {
        /// <summary>
        /// Point's departure name
        /// </summary>
        [Required]
        public string PointDepartureName { get; set; }

        /// <summary>
        /// Point's destiny name
        /// </summary>
        [Required]
        public string PointDestinyName { get; set; }

        /// <summary>
        /// Time spend to delivery a item between departure and destiny points
        /// </summary>
        [Range(1, 9999)]
        public int Time { get; set; }

        /// <summary>
        /// Cost to delivery a item between departure and destiny points
        /// </summary>
        [Range(1, 999999)]
        public int Cost { get; set; }
    }
}
