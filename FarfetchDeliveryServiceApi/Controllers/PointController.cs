using FarfetchDeliveryServiceApi.Models;
using FarfetchDeliveryServiceGraphRepository.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarfetchDeliveryServiceApi.Controllers
{
    /// <summary>
    /// Controller to maintain Points
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PointController : ControllerBase
    {
        private readonly IPointRepository _pointRepository;

        /// <summary>
        /// Default constructor
        /// </summary>
        public PointController(IPointRepository pointRepository)
        {
            _pointRepository = pointRepository;
        }

        /// <summary>
        /// Get all Points
        /// </summary>
        /// <returns>Points</returns>
        [HttpGet()]
        [Authorize(Roles = "user, admin")]
        public async Task<ActionResult> Get()
        {
            IEnumerable<string> points = await _pointRepository.Get();

            if (points == null || !points.Any())
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            return Ok(points);
        }

        /// <summary>
        /// Create a new Point
        /// </summary>
        /// <param name="name">New Point's name</param>
        [HttpPost("{name}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Post(string name)
        {
            bool exists = await _pointRepository.CheckIfExists(name);

            if (exists)
            {
                return StatusCode(StatusCodes.Status400BadRequest, $"The Point {name} already exists!");
            }

            await _pointRepository.Add(name);

            return Ok();
        }

        /// <summary>
        /// Delete a Point
        /// </summary>
        /// <param name="name">Point's Id that will be deleted</param>
        [HttpDelete("{name}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(string name)
        {
            bool exists = await _pointRepository.CheckIfExists(name);

            if (!exists)
            {
                return StatusCode(StatusCodes.Status400BadRequest, $"The Point {name} not exists!");
            }

            await _pointRepository.Delete(name);

            return Ok();
        }
    }
}
