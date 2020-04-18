using AutoMapper;
using FarfetchDeliveryServiceApi.Models;
using Entities = FarfetchDeliveryServiceGraphRepository.Entities;

namespace FarfetchDeliveryServiceApi.Mappers
{
    /// <summary>
    /// Class responsible to mapper Route model and entity
    /// </summary>
    public class RouteMapper : Profile
    {
        public RouteMapper()
        {
            CreateMap<Route, Entities.Route>().ReverseMap();
        }
    }
}
