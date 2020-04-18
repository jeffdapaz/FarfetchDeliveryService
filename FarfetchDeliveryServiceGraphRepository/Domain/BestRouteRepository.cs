using FarfetchDeliveryServiceGraphRepository.Domain.Interfaces;
using FarfetchDeliveryServiceGraphRepository.Entities;
using Neo4jClient;
using Neo4jClient.Cypher;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarfetchDeliveryServiceGraphRepository.Domain
{
    /// <summary>
    /// Repository for Best Route entity
    /// </summary>
    public class BestRouteRepository : IBestRouteRepository
    {
        private readonly IDatabaseGraphConnectionFactory _databaseConnectionFactory;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="databaseConnectionFactory">Factory for connection to the database</param>
        public BestRouteRepository(IDatabaseGraphConnectionFactory databaseConnectionFactory)
        {
            _databaseConnectionFactory = databaseConnectionFactory;
        }

        /// <summary>
        /// Get the best possible route between departure and final destiny points 
        /// </summary>
        /// <param name="pointDepartureName">Departure's name</param>
        /// <param name="pointDestinyName">Destiny's name</param>
        /// <returns>Best possible route between departure and final destiny points</returns>
        public async Task<BestRoute> Get(string pointDepartureName, string pointDestinyName)
        {
            IGraphClient client = _databaseConnectionFactory.GetConnection();

            var result = await client.Cypher
                .Match("(start:Point{Name:{PointDepartureName}})")
                .Match("(end:Point{Name:{PointDestinyName}})")
                .WithParam("PointDepartureName", pointDepartureName)
                .WithParam("PointDestinyName", pointDestinyName)
                .Call("algo.kShortestPaths.stream(start, end, 3, 'Effort' ,{})")
                .Yield("index, nodeIds, costs")
                .Return((index, nodeIds, costs) => new
                {
                    Routes = Return.As<List<string>>("[node in algo.getNodesById(nodeIds) | node.Name]"),
                    Efforts = costs.As<List<int>>(),
                    TotalEffort = Return.As<int>("reduce(acc = 0.0, cost in costs | acc + cost)")
                })
                .OrderBy("reduce(acc = 0.0, cost in costs | acc + cost)")
                .Limit(1)
                .ResultsAsync;

            var bestRouteResult = result.FirstOrDefault();

            if (bestRouteResult == null)
            {
                return null;
            }

            BestRoute bestRoute = new BestRoute()
            {
                PointDepartureName = pointDepartureName,
                PointDestinyName = pointDestinyName,
                TotalEffort = bestRouteResult.TotalEffort,
                CompletePath = new List<Path>()
            };

            for (int i = 0; i < bestRouteResult.Routes.Count; i++)
            {
                Path path = new Path()
                {
                    PointName = bestRouteResult.Routes[i],
                    Effort = i == 0 ? 0 : bestRouteResult.Efforts[i - 1]
                };

                bestRoute.CompletePath.Add(path);
            }

            return bestRoute;
        }
    }
}
