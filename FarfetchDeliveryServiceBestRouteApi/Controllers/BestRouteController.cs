using FarfetchDeliveryServiceBestRouteApi.Models;
using FarfetchDeliveryServiceGraphRepository.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FarfetchDeliveryServiceBestRouteApi.Controllers
{
    /// <summary>
    /// Controller responsible to get the best route
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BestRouteController : ControllerBase
    {
        private readonly string _token;
        private readonly IBestRouteRepository _bestRouteRepository;

        /// <summary>
        /// Default constructor
        /// </summary>
        public BestRouteController(IConfiguration configuration, IBestRouteRepository bestRouteRepository)
        {
            _token = configuration.GetSection("Token").Value;
            _bestRouteRepository = bestRouteRepository;
        }

        /// <summary>
        /// Action responsible to get the best route based on departure and destiny points
        /// </summary>
        /// <param name="token">Token header autorization</param>
        /// <param name="pointDepartureName">Departure's name</param>
        /// <param name="pointDestinyName">Destiny's name</param>
        /// <returns>Best route</returns>
        [HttpGet]
        public async Task<ActionResult> Get([FromHeader][Required]string token,
                                            [FromQuery][Required]string pointDepartureName,
                                            [FromQuery][Required]string pointDestinyName)
        {
            if (!token.Equals(_token))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Invalid Token!");
            }

            var bestRoute = await _bestRouteRepository.Get(pointDepartureName, pointDestinyName);

            if (bestRoute == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            BestRoute result = new BestRoute()
            {
                PointDepartureName = pointDepartureName,
                PointDestinyName = pointDestinyName,
                TotalEffort = bestRoute.TotalEffort,
                CompletePath = new List<Path>()
            };

            if (bestRoute.CompletePath != null)
            {
                foreach (var path in bestRoute.CompletePath)
                {
                    Path resultPath = new Path()
                    {
                        PointName = path.PointName,
                        Effort = path.Effort
                    };

                    result.CompletePath.Add(resultPath);
                }
            }

            return Ok(result);
        }
    }
}
