using AutoMapper;
using FarfetchDeliveryServiceApi.Mappers;
using FarfetchDeliveryServiceApi.Services;
using FarfetchDeliveryServiceApi.Services.Interfaces;
using FarfetchDeliveryServiceGraphRepository.Domain;
using FarfetchDeliveryServiceGraphRepository.Domain.Interfaces;
using FarfetchDeliveryServiceRepository.Domain;
using FarfetchDeliveryServiceSqlServerRepository.Domain;
using FarfetchDeliveryServiceSqlServerRepository.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FarfetchDeliveryServiceApi.Helpers
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
            services.AddScoped<IUsersServices, UsersServices>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IPointRepository, PointRepository>();
            services.AddScoped<IRouteRepository, RouteRepository>();

            services.AddScoped<IDatabaseSqlServerConnectionFactory>((connectionFactory) =>
            {
                string connectionString = configuration.GetConnectionString("FarfetchDeliveryService");

                return new DatabaseSqlServerConnectionFactory(connectionString);
            });

            services.AddScoped<IDatabaseGraphConnectionFactory>((connectionFactory) =>
            {
                string url = configuration.GetSection("Neo4j").GetSection("URL").Value;
                string userName = configuration.GetSection("Neo4j").GetSection("UserName").Value;
                string password = configuration.GetSection("Neo4j").GetSection("Password").Value;

                return new DatabaseGraphConnectionFactory(url, userName, password);
            });

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RouteMapper>();
            });

            services.AddSingleton(mapperConfig.CreateMapper());
        }
    }
}
