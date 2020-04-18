using FarfetchDeliveryServiceApi.Models;
using FarfetchDeliveryServiceApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FarfetchDeliveryServiceApi.Controllers
{
    /// <summary>
    /// Controller responsible for users autentication
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUsersServices _usersServices;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccountController(IUsersServices usersServices)
        {
            _usersServices = usersServices;
        }

        /// <summary>
        /// Action to do a user login
        /// </summary>
        /// <param name="user">User's data</param>
        /// <returns>Token to use in requests</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login([FromBody]User user)
        {
            string token = _usersServices.Authenticate(user);

            if (string.IsNullOrWhiteSpace(token))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Invalid User!");
            }

            return Ok(token);
        }
    }
}