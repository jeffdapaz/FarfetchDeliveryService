using System.ComponentModel.DataAnnotations;

namespace FarfetchDeliveryServiceApi.Models
{
    /// <summary>
    /// Model for the users
    /// </summary>
    public class User
    {
        /// <summary>
        /// User's login
        /// </summary>
        [Required]
        public string Login { get; set; }

        /// <summary>
        /// User's password
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
