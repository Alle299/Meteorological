using Meteorological.Enums;
using Meteorological.Models;
using Meteorological_API.Service;
using Meteorological_API.Service.Helpers;

namespace Meteorological_Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Normaly I don unittest api calls or database queries. But in this case I want to verify that the service is able to fetch data from the external API.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetDataByParameter_ReturnsWeatherReportAsync()
        {
            // Arrange
            var service = new WeatherReportService();

            // Act
            WeatherReport? result = await service.GetDataByParameter((Parameter)1, null, true);

            // Assert
            Assert.IsNotNull(result, "Expected non-null WeatherReport from GetDataByParameter");
            Assert.IsNotNull(result.Stations, "Expected the Stations collection to be initialized (not null)");
        }

        [Test]
        public void MessuringStation_GetValueUrlAugmentationOrEmpty_WhenStationKeyIsNull()
        {
            // Arrange
            long? stationKey = null;
            
            // Act
            var result = MeassuringStation.GetValueUrlAugmentationOrEmpty(stationKey);

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void MessuringStation_GetValueUrlAugmentationOrEmpty_WhenStationKeyIsZero()
        {
            // Arrange
            long? stationKey = 0;

            // Act
            var result = MeassuringStation.GetValueUrlAugmentationOrEmpty(stationKey);

            // Assert
            Assert.AreEqual(string.Empty, result);
        }


        [Test]
        public void MessuringStation_GetValueUrlAugmentationOrEmpty_WhenStationKeyIsNotZero()
        {
            // Arrange
            long? stationKey = 188790;

            // Act
            var result = MeassuringStation.GetValueUrlAugmentationOrEmpty(stationKey);

            // Assert
            Assert.AreEqual("/station/188790", result);
        }

        [Test]
        public void Scope_GetValueUrlAugmentationOrEmpty_WhenlatestDayIsNull()
        {
            // Arrange
            bool? latestDay = null;

            // Act
            var result = Scope.GetValueUrlAugmentationOrEmpty(latestDay);

            // Assert
            Assert.AreEqual("latest-hour", result);
        }

        [Test]
        public void Scope_GetValueUrlAugmentationOrEmpty_WhenlatestDayIsFalse()
        {
            // Arrange
            bool? latestDay = false;

            // Act
            var result = Scope.GetValueUrlAugmentationOrEmpty(latestDay);

            // Assert
            Assert.AreEqual("latest-hour", result);
        }

        [Test]
        public void Scope_GetValueUrlAugmentationOrEmpty_WhenlatestDayIsTrue()
        {
            // Arrange
            bool? latestDay = true;

            // Act
            var result = Scope.GetValueUrlAugmentationOrEmpty(latestDay);

            // Assert
            Assert.AreEqual("latest-day", result);
        }
    }
}