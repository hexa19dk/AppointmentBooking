using AgencyBookAppointments.Models;
using AgencyBookAppointments.Models.DTOs;
using AgencyBookAppointments.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgencyBookAppointments.Controllers
{
    [Route("api/agency")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(ILogger<AppointmentController> logger, IAppointmentService appointmentService)
        {
            _logger = logger;
            _appointmentService = appointmentService;
        }

        [HttpGet("/appointment/get-appointment/{date}")]
        public async Task<IActionResult> GetAppointmentsByDate(string date)
        {
            try
            {
                var result = await _appointmentService.ViewAppointmentsByDate(date);
                var response = ApiResponse<List<AppointmentResponseDto>>.Ok(result, "Appointments retrieved successfully");
                return StatusCode(response.StatusCode, response);
            }
            catch (FormatException ex)
            {
                var response = new ApiResponse<string>
                {
                    Success = false,
                    Message = "Invalid date format. Please use 'yyyy-MM-dd'.",
                    StatusCode = 400,
                    Data = null,
                    Error = ex.Message
                };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>
                {
                    Success = false,
                    Message = "An error occurred",
                    StatusCode = 500,
                    Data = null,
                    Error = ex.Message
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("/appointment/book-appointment")]
        public async Task<IActionResult> BookAppointment(AppointmentRequestDTO request)
        {
            try
            {
                var result = await _appointmentService.BookAppointment(request);
                var response = ApiResponse<AppointmentResponseDto>.Ok(result, "Appointment booked successfully");
                return StatusCode(response.StatusCode, response);
            }
            catch (ArgumentException ex)
            {
                var response = new ApiResponse<string>
                {
                    Success = false,
                    Message = ex.Message,
                    StatusCode = 400,
                    Data = null,
                    Error = ex.Message
                };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>
                {
                    Success = false,
                    Message = "An error occurred",
                    StatusCode = 500,
                    Data = null,
                    Error = ex.Message
                };
                return StatusCode(500, response);
            }
        }

        [HttpPut("/appointment/update-appointment")]
        public async Task<IActionResult> UpdateAppointmentStatus(int appointmentId, int status)
        {
            try
            {
                var result = await _appointmentService.UpdateAppointmentStatus(appointmentId, status);
                var response = ApiResponse<string>.Ok(result.ToString(), "Appointment updated successfully");
                return StatusCode(response.StatusCode, response);
            }
            catch (ArgumentException ex)
            {
                var response = new ApiResponse<string>
                {
                    Success = false,
                    Message = ex.Message,
                    StatusCode = 400,
                    Data = null,
                    Error = ex.Message
                };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>
                {
                    Success = false,
                    Message = "An error occurred",
                    StatusCode = 500,
                    Data = null,
                    Error = ex.Message
                };
                return StatusCode(500, response);
            }
        }
    }
}
