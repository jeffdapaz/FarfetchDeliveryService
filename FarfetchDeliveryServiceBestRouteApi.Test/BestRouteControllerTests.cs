using FarfetchDeliveryServiceBestRouteApi.Controllers;
using FarfetchDeliveryServiceGraphRepository.Domain.Interfaces;
using FarfetchDeliveryServiceGraphRepository.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace FarfetchDeliveryServiceBestRouteApi.Test
{
    /// <summary>
    /// Tests for the BestRouteController
    /// </summary>
    public class BestRouteControllerTests
    {
        /// <summary>
        /// Test Get method from BestRouteController with success
        /// </summary>
        [Fact]
        public void FarfetchDeliveryServiceBestRouteApi_BestRouteController_Get_WithSuccess()
        {
            Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();

            mockConfiguration.Setup(m => m.GetSection("Token")).Returns(new ConfigurationSection() { Value = "token" });

            Mock<IBestRouteRepository> mockRepository = new Mock<IBestRouteRepository>();

            BestRoute bestRoute = new BestRoute()
            {
                PointDepartureName = "A",
                PointDestinyName = "B"
            };

            mockRepository.Setup(m => m.Get(bestRoute.PointDepartureName, bestRoute.PointDestinyName)).ReturnsAsync(bestRoute);

            BestRouteController bestRouteController = new BestRouteController(mockConfiguration.Object, mockRepository.Object);

            var result = bestRouteController.Get("token", bestRoute.PointDepartureName, bestRoute.PointDestinyName).Result;

            OkObjectResult finalResult = result as OkObjectResult;

            Assert.NotNull(finalResult);
            Assert.NotNull(finalResult.Value);
            Assert.Equal(StatusCodes.Status200OK, finalResult.StatusCode);

            Models.BestRoute bestRouteResult = (Models.BestRoute)finalResult.Value;

            Assert.Equal(bestRoute.PointDepartureName, bestRouteResult.PointDepartureName);
            Assert.Equal(bestRoute.PointDestinyName, bestRouteResult.PointDestinyName);
        }

        /// <summary>
        /// Test Get method from BestRouteController without content
        /// </summary>
        [Fact]
        public void FarfetchDeliveryServiceBestRouteApi_BestRouteController_Get_WithoutContent()
        {
            Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();

            mockConfiguration.Setup(m => m.GetSection("Token")).Returns(new ConfigurationSection() { Value = "token" });

            Mock<IBestRouteRepository> mockRepository = new Mock<IBestRouteRepository>();

            mockRepository.Setup(m => m.Get(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((BestRoute)null);

            BestRouteController bestRouteController = new BestRouteController(mockConfiguration.Object, mockRepository.Object);

            var result = bestRouteController.Get("token", "", "").Result;

            StatusCodeResult finalResult = result as StatusCodeResult;

            Assert.NotNull(finalResult);
            Assert.Equal(StatusCodes.Status204NoContent, finalResult.StatusCode);
        }

        /// <summary>
        /// Test Get method from BestRouteController with invalid Token
        /// </summary>
        [Fact]
        public void FarfetchDeliveryServiceBestRouteApi_BestRouteController_Get_InvalidToken()
        {
            Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();

            mockConfiguration.Setup(m => m.GetSection("Token")).Returns(new ConfigurationSection() { Value = "token" });

            BestRouteController bestRouteController = new BestRouteController(mockConfiguration.Object, null);

            var result = bestRouteController.Get("", "", "").Result;

            ObjectResult finalResult = result as ObjectResult;

            Assert.NotNull(finalResult);
            Assert.Equal(StatusCodes.Status403Forbidden, finalResult.StatusCode);
        }
    }

    public class ConfigurationSection : IConfigurationSection
    {
        public string this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string Key { get; }

        public string Path { get; }

        public string Value { get; set; }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new NotImplementedException();
        }
    }
}
