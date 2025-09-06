using AgencyBookAppointments.Models;
using AgencyBookAppointments.Models.DTOs;
using AgencyBookAppointments.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgencyBookAppointments.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgencyController : ControllerBase
    {
        private readonly ILogger<AgencyController> _logger;
        private readonly IAgencyService _agencyService;
        public AgencyController(ILogger<AgencyController> logger, IAgencyService agencyService)
        {
            _logger = logger;
            _agencyService = agencyService;
        }

        [HttpPost("/agency/create-customer")]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerRequestDTO customer)
        {
            try
            {
                var result = await _agencyService.CreateCustomerAsync(customer);
                var response = ApiResponse<string>.Ok("Customer created successfully", "Success");
                return StatusCode(response.StatusCode, response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex.Message, "Validation error when creating customer");
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

        [HttpPost("/agency/create-holiday")]
        public async Task<IActionResult> CreateHoliday([FromBody] HolidayRequestDTO holiday)
        {
            try
            {
                var result = await _agencyService.CreateHolidayDate(holiday);
                var response = ApiResponse<string>.Ok("Holiday created successfully", "Success");
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

        [HttpPut("/agency/update-config")]
        public async Task<IActionResult> UpdateConfig([FromQuery] string keyName, [FromQuery] string keyValue)
        {
            try
            {
                await _agencyService.UpdateConfigAsync(keyName, keyValue);
                var response = ApiResponse<string>.Ok("Configuration updated successfully", "Success");
                return StatusCode(response.StatusCode, response);
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
