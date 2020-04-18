namespace FarfetchDeliveryServiceRepository.Entity
{
    /// <summary>
    /// Entity that represents a user
    /// </summary>
    public class Users
    {
        /// <summary>
        /// User's login
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// User's password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// User's role
        /// </summary>
        public string Role { get; set; }
    }
}
