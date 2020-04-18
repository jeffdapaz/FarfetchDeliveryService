using FarfetchDeliveryServiceGraphRepository.Domain.Interfaces;
using Neo4jClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarfetchDeliveryServiceGraphRepository.Domain
{
    /// <summary>
    /// Repository for Point entity
    /// </summary>
    public class PointRepository : IPointRepository
    {
        /// <summary>
        /// Factory for connection to the database
        /// </summary>
        protected readonly IDatabaseGraphConnectionFactory _databaseConnectionFactory;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="databaseConnectionFactory">Factory for connection to the database</param>
        public PointRepository(IDatabaseGraphConnectionFactory databaseConnectionFactory)
        {
            _databaseConnectionFactory = databaseConnectionFactory;
        }

        /// <summary>
        /// Get all points from the database
        /// </summary>
        /// <returns>Points</returns>
        public virtual async Task<IEnumerable<string>> Get()
        {
            IGraphClient client = _databaseConnectionFactory.GetConnection();

            return await client.Cypher
                            .Match("(point:Point)")
                            .Return<string>("point.Name")
                            .OrderBy("point.Name")
                            .ResultsAsync;
        }

        /// <summary>
        /// Check if a point exists
        /// </summary>
        /// <returns>True if a point already exists</returns>
        public virtual async Task<bool> CheckIfExists(string name)
        {
            IGraphClient client = _databaseConnectionFactory.GetConnection();

            var points = await client.Cypher
                            .Match("(point:Point)")
                            .Where("point.Name = {name}")
                            .WithParam("name", name)
                            .Return<string>("point.Name")
                            .Limit(1)
                            .ResultsAsync;

            return points != null && points.Any();
        }

        /// <summary>
        /// Add a new point to the database
        /// </summary>
        /// <param name="point">Point's name for the new point</param>
        public async Task Add(string name)
        {
            IGraphClient client = _databaseConnectionFactory.GetConnection();

            await client.Cypher
                    .Create("(point:Point {Name: {name}})")
                    .WithParam("name", name)
                    .ExecuteWithoutResultsAsync();
        }

        /// <summary>
        /// Remove a point from database
        /// </summary>
        /// <param name="name">Point's name thaat will be deleted</param>
        public async Task Delete(string name)
        {
            IGraphClient client = _databaseConnectionFactory.GetConnection();

            await client.Cypher
                .Match("(point {Name: {name}})")
                .WithParam("name", name)
                .DetachDelete("point")
                .ExecuteWithoutResultsAsync();
        }
    }
}
