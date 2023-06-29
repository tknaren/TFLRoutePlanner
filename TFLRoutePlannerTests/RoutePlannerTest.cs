using Microsoft.VisualStudio.TestTools.UnitTesting;
using TFLRoutePlanner.Utilities;
using TFLRoutePlannerBL;
using TFLRoutePlannerData;
using TFLRoutePlannerData.Model;

namespace TFLRoutePlannerTests
{
    [TestClass]
    public class RoutePlannerTest
    {
        private readonly ConfigSettings _configSettings;
        private readonly ITFLRoutePlannerDB _routesDB;
        private readonly IRoutePlannerLogic _routePlanner;

        public RoutePlannerTest(ConfigSettings configSettings, ITFLRoutePlannerDB routesDB)
        {
            _configSettings = configSettings;
            _routesDB = new TFLRoutePlannerDB(_configSettings);
            _routePlanner = new RoutePlannerLogic(_configSettings);
        }

        [TestMethod]
        public void GenerateRoute_WithStartAndEndStation_ReturnsResponse()
        {
            // Arrange
            RouteRequest request = new RouteRequest()
            {
                source = "HOLBORN",
                destination = "CHARING CROSS"
            };

            // Act
            RouteResponse response = _routePlanner.GenerateRoute(request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.line.Count > 1);
            Assert.IsTrue(response.route.Count > 1);
        }

        [TestMethod]
        public void GenerateRoute_WithStartEndAndViaStation_ReturnsResponse()
        {
            // Arrange
            RouteRequest request = new RouteRequest()
            {
                source = "HOLBORN",
                destination = "CHARING CROSS",
                viaStation = "COVENT GARDEN"
            };

            // Act
            RouteResponse response = _routePlanner.GenerateRoute(request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.line.Count > 1);
            Assert.IsTrue(response.route.Count > 1);
        }

        [TestMethod]
        public void GenerateRoute_WithStartEndAndExclStation_ReturnsResponse()
        {
            // Arrange
            RouteRequest request = new RouteRequest()
            {
                source = "HOLBORN",
                destination = "CHARING CROSS",
                excludeStation = "COVENT GARDEN"
            };

            // Act
            RouteResponse response = _routePlanner.GenerateRoute(request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.line.Count > 1);
            Assert.IsTrue(response.route.Count > 1);
        }

        [TestMethod]
        public void GenerateRoute_WithStartEndViaAndExclStation_ReturnsResponse()
        {
            // Arrange
            RouteRequest request = new RouteRequest()
            {
                source = "HOLBORN",
                destination = "CHARING CROSS",
                viaStation = "CANNON STREET",
                excludeStation = "COVENT GARDEN"
            };

            // Act
            RouteResponse response = _routePlanner.GenerateRoute(request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.line.Count > 1);
            Assert.IsTrue(response.route.Count > 1);
        }

        [TestMethod]
        public void GenerateRoute_WithStartWithoutEnd_ReturnsErrorResponse()
        {
            // Arrange
            RouteRequest request = new RouteRequest()
            {
                source = "HOLBORN",
                destination = ""
            };

            // Act
            RouteResponse response = _routePlanner.GenerateRoute(request);

            // Assert
            Assert.IsNull(response);
            Assert.IsFalse(response.line.Count > 1);
            Assert.IsFalse(response.route.Count > 1);
        }

        [TestMethod]
        public void GenerateRoute_WithoutStartWithEnd_ReturnsErrorResponse()
        {
            // Arrange
            RouteRequest request = new RouteRequest()
            {
                source = "",
                destination = "HOLBORN"
            };

            // Act
            RouteResponse response = _routePlanner.GenerateRoute(request);

            // Assert
            Assert.IsNull(response);
            Assert.IsFalse(response.line.Count > 1);
            Assert.IsFalse(response.route.Count > 1);
        }

    }
}