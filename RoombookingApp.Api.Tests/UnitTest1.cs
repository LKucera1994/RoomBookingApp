using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using RombookingApp.Api.Controllers;
using Shouldly;

namespace RoombookingApp.Api.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void ShouldReturnForecastResults()
        {
            var loggerMock = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(loggerMock.Object);

            var result = controller.Get();

            result.ShouldNotBeNull();
            result.Count().ShouldBeGreaterThan(1);
            
        }
    }
}