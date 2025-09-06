using System;
using System.Collections.Generic;
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
    public class AppointmentControllerTests
    {
        [Fact]
        public async Task BookAppointment_Returns200_WithAppointment()
        {
            var req = new AppointmentRequestDTO { AppointmentDate = "2025-09-06", CustomerId = 1, Notes = "", ServiceType = 1 };
            var resp = new AppointmentResponseDto { Id = 1, AppointmentDate = "2025-09-06", AppointmentNumber = "APT-20250906-001", CustomerId = 1, Notes = "", ServiceType = 1 };

            var serviceMock = new Mock<IAppointmentService>();
            serviceMock.Setup(s => s.BookAppointment(req)).ReturnsAsync(resp);

            var loggerMock = new Mock<ILogger<AppointmentController>>();
            var controller = new AppointmentController(loggerMock.Object, serviceMock.Object);

            var result = await controller.BookAppointment(req);

            var objResult = result as ObjectResult;
            objResult.Should().NotBeNull();
            objResult!.StatusCode.Should().Be(200);

            var apiResp = objResult.Value as AgencyBookAppointments.Models.ApiResponse<AppointmentResponseDto>;
            apiResp.Should().NotBeNull();
            apiResp!.Success.Should().BeTrue();
            apiResp.Data.Should().NotBeNull();
            apiResp.Data!.Id.Should().Be(1);
        }

        [Fact]
        public async Task BookAppointment_ReturnsBadRequest_WhenServiceThrowsArgumentException()
        {
            var req = new AppointmentRequestDTO { AppointmentDate = "2025-09-06", CustomerId = 1, Notes = "", ServiceType = 1 };

            var serviceMock = new Mock<IAppointmentService>();
            serviceMock.Setup(s => s.BookAppointment(req)).ThrowsAsync(new ArgumentException("Invalid date"));

            var loggerMock = new Mock<ILogger<AppointmentController>>();
            var controller = new AppointmentController(loggerMock.Object, serviceMock.Object);

            var result = await controller.BookAppointment(req);

            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest!.StatusCode.Should().Be(400);

            var apiResp = badRequest.Value as AgencyBookAppointments.Models.ApiResponse<string>;
            apiResp.Should().NotBeNull();
            apiResp!.Success.Should().BeFalse();
            apiResp.Message.Should().Be("Invalid date");
        }

        [Fact]
        public async Task GetAppointmentsByDate_ReturnsList()
        {
            var date = "2025-09-06";
            var list = new List<AppointmentResponseDto>
            {
                new AppointmentResponseDto { Id = 1, AppointmentDate = date, AppointmentNumber = "APT-20250906-001", CustomerId = 1, Notes = "", ServiceType = 1 }
            };

            var serviceMock = new Mock<IAppointmentService>();
            serviceMock.Setup(s => s.ViewAppointmentsByDate(date)).ReturnsAsync(list);

            var loggerMock = new Mock<ILogger<AppointmentController>>();
            var controller = new AppointmentController(loggerMock.Object, serviceMock.Object);

            var result = await controller.GetAppointmentsByDate(date);

            var objResult = result as ObjectResult;
            objResult.Should().NotBeNull();
            objResult!.StatusCode.Should().Be(200);

            var apiResp = objResult.Value as AgencyBookAppointments.Models.ApiResponse<List<AppointmentResponseDto>>;
            apiResp.Should().NotBeNull();
            apiResp!.Success.Should().BeTrue();
            apiResp.Data.Should().HaveCount(1);
        }
    }
}
