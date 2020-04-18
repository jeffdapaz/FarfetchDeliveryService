using FarfetchDeliveryServiceGraphRepository.Domain;
using FarfetchDeliveryServiceGraphRepository.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FarfetchDeliveryServiceBestRouteApi.Helpers
{
    /// <summary>
    /// Class responsible for the project dependecy injection
    /// </summary>
    public class DependencyInjection
    {
        /// <summary>
        /// Configure the project dependecy injection
        /// </summary>
        public static void Configure(IConfiguration configuration, IServiceCollection services)
        {
            services.AddScoped<IBestRouteRepository, BestRouteRepository>();

            services.AddScoped<IDatabaseGraphConnectionFactory>((connectionFactory) =>
            {
                string url = configuration.GetSection("Neo4j").GetSection("URL").Value;
                string userName = configuration.GetSection("Neo4j").GetSection("UserName").Value;
                string password = configuration.GetSection("Neo4j").GetSection("Password").Value;

                return new DatabaseGraphConnectionFactory(url, userName, password);
            });
        }
    }
}
