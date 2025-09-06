using System.Threading.Tasks;
using AgencyBookAppointments.Controllers;
using AgencyBookAppointments.Models.DTOs;
using AgencyBookAppointments.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AgencyBookAppointments.Tests.Controllers
{
    public class AgencyControllerTests
    {
        [Fact]
        public async Task CreateCustomer_Returns200_WhenServiceSucceeds()
        {
            var dto = new CustomerRequestDTO { FullName = "Ben Barlow", Email = "ben.barlow@testing.com", PhoneNumber = "+6289998888" };
            var serviceMock = new Mock<IAgencyService>();
            serviceMock.Setup(s => s.CreateCustomerAsync(dto)).ReturnsAsync(true);

            var loggerMock = new Mock<ILogger<AgencyController>>();
            var controller = new AgencyController(loggerMock.Object, serviceMock.Object);

            var result = await controller.CreateCustomer(dto);

            var objResult = result as ObjectResult;
            objResult.Should().NotBeNull();
            objResult!.StatusCode.Should().Be(200);

            var apiResp = objResult.Value as AgencyBookAppointments.Models.ApiResponse<string>;
            apiResp.Should().NotBeNull();
            apiResp!.Success.Should().BeTrue();
            apiResp.Data.Should().Be("Customer created successfully");
        }

        [Fact]
        public async Task CreateCustomer_ReturnsBadRequest_WhenServiceThrowsArgumentException()
        {
            var dto = new CustomerRequestDTO { FullName = "Jane", Email = "jane@example.com", PhoneNumber = "+6289998888" };
            var serviceMock = new Mock<IAgencyService>();
            serviceMock.Setup(s => s.CreateCustomerAsync(dto)).ThrowsAsync(new System.ArgumentException("Invalid email"));

            var loggerMock = new Mock<ILogger<AgencyController>>();
            var controller = new AgencyController(loggerMock.Object, serviceMock.Object);

            var result = await controller.CreateCustomer(dto);

            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest!.StatusCode.Should().Be(400);

            var apiResp = badRequest.Value as AgencyBookAppointments.Models.ApiResponse<string>;
            apiResp.Should().NotBeNull();
            apiResp!.Success.Should().BeFalse();
            apiResp.Message.Should().Be("Invalid email");
        }
    }
}
