using AutoMapper;
using FarfetchDeliveryServiceApi.Controllers;
using FarfetchDeliveryServiceApi.Mappers;
using FarfetchDeliveryServiceGraphRepository.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;
using Entities = FarfetchDeliveryServiceGraphRepository.Entities;

namespace FarfetchDeliveryServiceApi.Test
{
    /// <summary>
    /// Tests for the RouteController
    /// </summary>
    public class RouteControllerTests
    {
        private readonly Mock<IRouteRepository> _mockRouteRepository;
        private readonly Mock<IPointRepository> _mockPointRepository;
        private readonly RouteController _controller;

        /// <summary>
        /// Default constructor
        /// </summary>
        public RouteControllerTests()
        {
            _mockRouteRepository = new Mock<IRouteRepository>();
            _mockPointRepository = new Mock<IPointRepository>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RouteMapper>();
            });

            _controller = new RouteController(_mockRouteRepository.Object, _mockPointRepository.Object, mapperConfig.CreateMapper());
        }

        /// <summary>
        /// Test Get method from RouteController with success
        /// </summary>
        [Fact]
        public void FarfetchDeliveryServiceApi_RouteController_Get_WithSuccess()
        {
            Entities.Route route = new Entities.Route()
            {
                PointDepartureName = "A",
                PointDestinyName = "B"
            };

            _mockRouteRepository.Setup(m => m.Get("A", "B")).ReturnsAsync(route);

            var result = _controller.Get("A", "B").Result;

            OkObjectResult finalResult = result as OkObjectResult;

            Assert.NotNull(finalResult);
            Assert.NotNull(finalResult.Value);
            Assert.Equal(StatusCodes.Status200OK, finalResult.StatusCode);
        }

        /// <summary>
        /// Test GetByDeparture method from RouteController with success
        /// </summary>
        [Fact]
        public void FarfetchDeliveryServiceApi_RouteController_GetByDeparture_WithSuccess()
        {
            Entities.Route routeAB = new Entities.Route()
            {
                PointDepartureName = "A",
                PointDestinyName = "B"
            };

            Entities.Route routeAC = new Entities.Route()
            {
                PointDepartureName = "A",
                PointDestinyName = "C"
            };

            List<Entities.Route> routes = new List<Entities.Route>() { routeAB, routeAC };

            _mockRouteRepository.Setup(m => m.GetByDeparture("A")).ReturnsAsync(routes);

            var result = _controller.GetByDeparture("A").Result;

            OkObjectResult finalResult = result as OkObjectResult;

            Assert.NotNull(finalResult);
            Assert.NotNull(finalResult.Value);
            Assert.Equal(StatusCodes.Status200OK, finalResult.StatusCode);

            List<Models.Route> routesResult = (List<Models.Route>)finalResult.Value;

            Assert.Equal(routes.Count, routesResult.Count);
        }

        /// <summary>
        /// Test GetByDestiny method from RouteController with success
        /// </summary>
        [Fact]
        public void FarfetchDeliveryServiceApi_RouteController_GetByDestiny_WithSuccess()
        {
            Entities.Route routeAB = new Entities.Route()
            {
                PointDepartureName = "B",
                PointDestinyName = "A"
            };

            Entities.Route routeAC = new Entities.Route()
            {
                PointDepartureName = "C",
                PointDestinyName = "A"
            };

            List<Entities.Route> routes = new List<Entities.Route>() { routeAB, routeAC };

            _mockRouteRepository.Setup(m => m.GetByDestiny("A")).ReturnsAsync(routes);

            var result = _controller.GetByDestiny("A").Result;

            OkObjectResult finalResult = result as OkObjectResult;

            Assert.NotNull(finalResult);
            Assert.NotNull(finalResult.Value);
            Assert.Equal(StatusCodes.Status200OK, finalResult.StatusCode);

            List<Models.Route> routesResult = (List<Models.Route>)finalResult.Value;

            Assert.Equal(routes.Count, routesResult.Count);
        }

        /// <summary>
        /// Test Post method from RouteController with success
        /// </summary>
        [Fact]
        public void FarfetchDeliveryServiceApi_RouteController_Post_WithSuccess()
        {
            _mockPointRepository.Setup(m => m.CheckIfExists(It.IsAny<string>())).ReturnsAsync(true);

            var result = _controller.Post(new Models.Route()).Result;

            _mockPointRepository.Verify(m => m.CheckIfExists(It.IsAny<string>()), Times.Exactly(2));
            _mockRouteRepository.Verify(m => m.Get(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _mockRouteRepository.Verify(m => m.Add(It.IsAny<Entities.Route>()), Times.Once);

            OkResult finalResult = result as OkResult;

            Assert.NotNull(finalResult);
            Assert.Equal(StatusCodes.Status200OK, finalResult.StatusCode);
        }

        /// <summary>
        /// Test Post method from RouteController with where a point not existss
        /// </summary>
        [Fact]
        public void FarfetchDeliveryServiceApi_RouteController_Post_PointNotExistss()
        {
            _mockPointRepository.Setup(m => m.CheckIfExists(It.IsAny<string>())).ReturnsAsync(false);

            var result = _controller.Post(new Models.Route()).Result;

            _mockPointRepository.Verify(m => m.CheckIfExists(It.IsAny<string>()), Times.Once);
            _mockRouteRepository.Verify(m => m.Get(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _mockRouteRepository.Verify(m => m.Add(It.IsAny<Entities.Route>()), Times.Never);

            ObjectResult finalResult = result as ObjectResult;

            Assert.NotNull(finalResult);
            Assert.Equal(StatusCodes.Status400BadRequest, finalResult.StatusCode);
        }

        /// <summary>
        /// Test Post method from RouteController where route already exists
        /// </summary>
        [Fact]
        public void FarfetchDeliveryServiceApi_RouteController_Post_RouteAlreadyExists()
        {
            _mockPointRepository.Setup(m => m.CheckIfExists(It.IsAny<string>())).ReturnsAsync(true);
            _mockRouteRepository.Setup(m => m.Get(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new Entities.Route());

            var result = _controller.Post(new Models.Route()).Result;

            _mockPointRepository.Verify(m => m.CheckIfExists(It.IsAny<string>()), Times.Exactly(2));
            _mockRouteRepository.Verify(m => m.Get(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _mockRouteRepository.Verify(m => m.Add(It.IsAny<Entities.Route>()), Times.Never);

            ObjectResult finalResult = result as ObjectResult;

            Assert.NotNull(finalResult);
            Assert.Equal(StatusCodes.Status400BadRequest, finalResult.StatusCode);
        }

        /// <summary>
        /// Test Put method from RouteController with success
        /// </summary>
        [Fact]
        public void FarfetchDeliveryServiceApi_RouteController_Put_WithSuccess()
        {
            _mockRouteRepository.Setup(m => m.Get(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new Entities.Route());

            var result = _controller.Put(new Models.Route()).Result;

            _mockRouteRepository.Verify(m => m.Get(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _mockRouteRepository.Verify(m => m.Update(It.IsAny<Entities.Route>()), Times.Once);

            OkResult finalResult = result as OkResult;

            Assert.NotNull(finalResult);
            Assert.Equal(StatusCodes.Status200OK, finalResult.StatusCode);
        }

        /// <summary>
        /// Test Put method from RouteController where route no exists
        /// </summary>
        [Fact]
        public void FarfetchDeliveryServiceApi_RouteController_Put_RouteNotExists()
        {
            _mockRouteRepository.Setup(m => m.Get(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((Entities.Route)null);

            var result = _controller.Put(new Models.Route()).Result;

            _mockRouteRepository.Verify(m => m.Get(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _mockRouteRepository.Verify(m => m.Update(It.IsAny<Entities.Route>()), Times.Never);

            ObjectResult finalResult = result as ObjectResult;

            Assert.NotNull(finalResult);
            Assert.Equal(StatusCodes.Status400BadRequest, finalResult.StatusCode);
        }

        /// <summary>
        /// Test Delete method from RouteController with success
        /// </summary>
        [Fact]
        public void FarfetchDeliveryServiceApi_RouteController_Delete_WithSuccess()
        {
            _mockRouteRepository.Setup(m => m.Get(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new Entities.Route());

            var result = _controller.Delete("", "").Result;

            _mockRouteRepository.Verify(m => m.Get(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _mockRouteRepository.Verify(m => m.Delete(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            OkResult finalResult = result as OkResult;

            Assert.NotNull(finalResult);
            Assert.Equal(StatusCodes.Status200OK, finalResult.StatusCode);
        }

        /// <summary>
        /// Test Put method from RouteController where route no exists
        /// </summary>
        [Fact]
        public void FarfetchDeliveryServiceApi_RouteController_Delete_RouteNotExists()
        {
            _mockRouteRepository.Setup(m => m.Get(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((Entities.Route)null);

            var result = _controller.Delete("", "").Result;

            _mockRouteRepository.Verify(m => m.Get(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _mockRouteRepository.Verify(m => m.Delete(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

            ObjectResult finalResult = result as ObjectResult;

            Assert.NotNull(finalResult);
            Assert.Equal(StatusCodes.Status400BadRequest, finalResult.StatusCode);
        }
    }
}
