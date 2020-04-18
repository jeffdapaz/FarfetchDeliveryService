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
    /// Repository for Route entity
    /// </summary>
    public class RouteRepository : IRouteRepository
    {
        /// <summary>
        /// Factory for connection to the database
        /// </summary>
        protected readonly IDatabaseGraphConnectionFactory _databaseConnectionFactory;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="databaseConnectionFactory">Factory for connection to the database</param>
        public RouteRepository(IDatabaseGraphConnectionFactory databaseConnectionFactory)
        {
            _databaseConnectionFactory = databaseConnectionFactory;
        }

        /// <summary>
        /// Get a route between two points
        /// </summary>
        /// <param name="pointDepartureName">Departure's name</param>
        /// <param name="pointDestinyName">Destiny's name</param>
        /// <returns>Route</returns>
        public async Task<Route> Get(string pointDepartureName, string pointDestinyName)
        {
            IGraphClient client = _databaseConnectionFactory.GetConnection();

            IEnumerable<Route> routes = await client.Cypher
                        .Match("(start:Point)-[route:Route]-(end:Point)")
                        .Where("start.Name = {PointDepartureName} AND end.Name = {PointDestinyName}")
                        .WithParam("PointDepartureName", pointDepartureName)
                        .WithParam("PointDestinyName", pointDestinyName)
                        .Return((start, route, end) => new Route
                        {
                            PointDepartureName = Return.As<string>("start.Name"),
                            PointDestinyName = Return.As<string>("end.Name"),
                            Cost = Return.As<int>("route.Cost"),
                            Time = Return.As<int>("route.Time")
                        })
                        .Limit(1)
                        .ResultsAsync;

            return routes.FirstOrDefault();
        }

        /// <summary>
        /// Return a route list that have a point as departure
        /// </summary>
        /// <param name="pointDepartureName">Departure's name</param>
        /// <returns>Route list that have a point as departure</returns>
        public async Task<IEnumerable<Route>> GetByDeparture(string pointDepartureName)
        {
            IGraphClient client = _databaseConnectionFactory.GetConnection();

            return await client.Cypher
                            .Match("(start:Point)-[route:Route]->(end:Point)")
                            .Where("start.Name = {PointDepartureName}")
                            .WithParam("PointDepartureName", pointDepartureName)
                            .Return((start, route, end) => new Route
                            {
                                PointDepartureName = Return.As<string>("start.Name"),
                                PointDestinyName = Return.As<string>("end.Name"),
                                Cost = Return.As<int>("route.Cost"),
                                Time = Return.As<int>("route.Time")
                            })
                            .ResultsAsync;
        }

        /// <summary>
        /// Return a route list that have a point as destiny
        /// </summary>
        /// <param name="pointDestinyName">Destiny's name</param>
        /// <returns>Route list that have a point as destiny</returns>
        public async Task<IEnumerable<Route>> GetByDestiny(string pointDestinyName)
        {
            IGraphClient client = _databaseConnectionFactory.GetConnection();

            return await client.Cypher
                            .Match("(start:Point)-[route:Route]->(end:Point)")
                            .Where("end.Name = {PointDestinyName}")
                            .WithParam("PointDestinyName", pointDestinyName)
                            .Return((start, route, end) => new Route
                            {
                                PointDepartureName = Return.As<string>("start.Name"),
                                PointDestinyName = Return.As<string>("end.Name"),
                                Cost = Return.As<int>("route.Cost"),
                                Time = Return.As<int>("route.Time")
                            })
                            .ResultsAsync;
        }

        /// <summary>
        /// Add a new route to the database
        /// </summary>
        /// <param name="route">Route that will be added</param>
        public async Task Add(Route route)
        {
            IGraphClient client = _databaseConnectionFactory.GetConnection();

            await client.Cypher
                .Match("(start:Point)", "(end:Point)")
                .Where("start.Name = {PointDepartureName} AND end.Name = {PointDestinyName}")
                .WithParam("PointDepartureName", route.PointDepartureName)
                .WithParam("PointDestinyName", route.PointDestinyName)
                .CreateUnique("(start)-[:Route {Time:{time}, Cost:{cost}, Effort:{effort}}]->(end)")
                .WithParams(new
                {
                    time = route.Time,
                    cost = route.Cost,
                    effort = route.Time * route.Cost
                })
                .ExecuteWithoutResultsAsync();
        }

        /// <summary>
        /// Update a existent route
        /// </summary>
        /// <param name="route">Route that will be updated</param>
        public async Task Update(Route route)
        {
            IGraphClient client = _databaseConnectionFactory.GetConnection();

            await client.Cypher
                .Match("(start:Point)-[route:Route]-(end:Point)")
                .Where("start.Name = {PointDepartureName} AND end.Name = {PointDestinyName}")
                .WithParam("PointDepartureName", route.PointDepartureName)
                .WithParam("PointDestinyName", route.PointDestinyName)
                .Set("route.Cost = {Cost}, route.Time = {Time}, route.Effort = {Effort}")
                .WithParam("Cost", route.Cost)
                .WithParam("Time", route.Time)
                .WithParam("Effort", route.Cost * route.Time)
                .ExecuteWithoutResultsAsync();
        }

        /// <summary>
        /// Remove a route between two points from the database
        /// </summary>
        /// <param name="pointDepartureName">Departure's name</param>
        /// <param name="pointDestinyName">Destiny's name</param>
        public async Task Delete(string pointDepartureName, string pointDestinyName)
        {
            IGraphClient client = _databaseConnectionFactory.GetConnection();

            await client.Cypher
                .Match("(start:Point)-[route:Route]-(end:Point)")
                .Where("start.Name = {PointDepartureName} AND end.Name = {PointDestinyName}")
                .WithParam("PointDepartureName", pointDepartureName)
                .WithParam("PointDestinyName", pointDestinyName)
                .Delete("route")
                .ExecuteWithoutResultsAsync();
        }
    }
}
