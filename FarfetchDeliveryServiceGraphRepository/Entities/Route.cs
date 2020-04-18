namespace FarfetchDeliveryServiceGraphRepository.Entities
{
    /// <summary>
    /// Entity that represents the route between a departure and destiny points
    /// </summary>
    public class Route
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
        /// Time spend to delivery a item between departure and destiny points
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// Cost to delivery a item between departure and destiny points
        /// </summary>
        public int Cost { get; set; }
    }
}
