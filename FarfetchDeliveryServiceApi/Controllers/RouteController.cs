using AutoMapper;
using FarfetchDeliveryServiceApi.Models;
using FarfetchDeliveryServiceGraphRepository.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Entities = FarfetchDeliveryServiceGraphRepository.Entities;

namespace FarfetchDeliveryServiceApi.Controllers
{
    /// <summary>
    /// Controller to maintain Routes
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly IRouteRepository _routeRepository;
        private readonly IPointRepository _pointRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Default constructor
        /// </summary>
        public RouteController(IRouteRepository routeRepository, IPointRepository pointRepository, IMapper mapper)
        {
            _routeRepository = routeRepository;
            _pointRepository = pointRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a route between two points
        /// </summary>
        /// <param name="pointDepartureName">Departure's name</param>
        /// <param name="pointDestinyName">Destiny's name</param>
        /// <returns>Route</returns>
        [HttpGet()]
        [Authorize(Roles = "user, admin")]
        public async Task<ActionResult> Get([FromQuery][Required]string pointDepartureName,
                                            [FromQuery][Required]string pointDestinyName)
        {
            Entities.Route route = await _routeRepository.Get(pointDepartureName, pointDestinyName);

            if (route == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            return Ok(_mapper.Map<Route>(route));
        }

        /// <summary>
        /// Return a route list that have a point as departure
        /// </summary>
        /// <param name="pointDepartureName">Departure's name</param>
        /// <returns>Route list that have a point as departure</returns>
        [HttpGet("GetByDeparture/{pointDepartureName}")]
        [Authorize(Roles = "user, admin")]
        public async Task<ActionResult> GetByDeparture(string pointDepartureName)
        {
            IEnumerable<Entities.Route> routes = await _routeRepository.GetByDeparture(pointDepartureName);

            if (routes == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            return Ok(_mapper.Map<IEnumerable<Route>>(routes));
        }

        /// <summary>
        /// Return a route list that have a point as destiny
        /// </summary>
        /// <param name="pointDestinyName">Destiny's name</param>
        /// <returns>Route list that have a point as destiny</returns>
        [HttpGet("GetByDestiny/{pointDestinyName}")]
        [Authorize(Roles = "user, admin")]
        public async Task<ActionResult> GetByDestiny(string pointDestinyName)
        {
            IEnumerable<Entities.Route> routes = await _routeRepository.GetByDestiny(pointDestinyName);

            if (routes == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            return Ok(_mapper.Map<IEnumerable<Route>>(routes));
        }

        /// <summary>
        /// Create a new Route
        /// </summary>
        /// <param name="route">Route that will be created</param>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Post([FromBody]Route route)
        {
            if (!_pointRepository.CheckIfExists(route.PointDepartureName).Result)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "The Point departure not exists!");
            }

            if (!_pointRepository.CheckIfExists(route.PointDestinyName).Result)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "The Point destiny not exists!");
            }

            Entities.Route routeExistent = await _routeRepository.Get(route.PointDepartureName, route.PointDestinyName);

            if (routeExistent != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "This Route already exists!");
            }

            await _routeRepository.Add(_mapper.Map<Entities.Route>(route));

            return Ok();
        }

        /// <summary>
        /// Update a Route
        /// </summary>
        /// <param name="route">Route that will be updated</param>
        [HttpPut()]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Put([FromBody]Route route)
        {
            Entities.Route routeExistent = await _routeRepository.Get(route.PointDepartureName, route.PointDestinyName);

            if (routeExistent == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "This Route not exists!");
            }

            await _routeRepository.Update(_mapper.Map<Entities.Route>(route));

            return Ok();
        }

        /// <summary>
        /// Delete a Route
        /// </summary>
        /// <param name="pointDepartureName">Departure's name</param>
        /// <param name="pointDestinyName">Destiny's name</param>
        [HttpDelete()]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete([FromQuery][Required]string pointDepartureName,
                                               [FromQuery][Required]string pointDestinyName)
        {
            Entities.Route routeExistent = await _routeRepository.Get(pointDepartureName, pointDestinyName);

            if (routeExistent == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "This Route not exists!");
            }

            await _routeRepository.Delete(pointDepartureName, pointDestinyName);

            return Ok();
        }
    }
}
