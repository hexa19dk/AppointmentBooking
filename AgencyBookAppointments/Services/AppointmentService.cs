using AgencyBookAppointments.Models;
using AgencyBookAppointments.Models.DTOs;
using AgencyBookAppointments.Repositories;
using System.Reflection.Metadata.Ecma335;

namespace AgencyBookAppointments.Services
{
    public interface IAppointmentService
    {
        Task<AppointmentResponseDto> BookAppointment(AppointmentRequestDTO request);
        Task<List<AppointmentResponseDto>> ViewAppointmentsByDate(string appointmentDate);
        Task<bool> UpdateAppointmentStatus(int appointmentId, int status);
    }

    public class AppointmentService : IAppointmentService
    {
        private readonly ILogger<AppointmentService> _logger;
        private readonly IAppointmentRepositories _aptRepo;
        private readonly IHolidayRepositories _holidayRepo;
        private readonly IConfigurationRepositories _configRepo;
        public AppointmentService(ILogger<AppointmentService> logger, IAppointmentRepositories aptRepo, IHolidayRepositories holidayRepo, IConfigurationRepositories configRepo)
        {
            _logger = logger;
            _aptRepo = aptRepo;
            _holidayRepo = holidayRepo;
            _configRepo = configRepo;
        }

        public async Task<AppointmentResponseDto> BookAppointment(AppointmentRequestDTO request)
        {
            try
            {
                var dateAssign = DateTime.Parse(request.AppointmentDate!);

                // Check Appointment Backdate
                if (dateAssign < DateTime.UtcNow.Date)
                {
                    throw new ArgumentException($"Appointments cannot be booked in the past. Please select {DateTime.UtcNow:yyyy-MM-dd} or later.");
                }

                // Check Holiday Dates
                var checkHoliday = await _holidayRepo.CheckHolidayDates(dateAssign);
                if (checkHoliday != null)
                {
                    throw new ArgumentException($"Sorry, we're closed on {checkHoliday.HolidayDate.Date} for {checkHoliday.Description}. Please select another date.");
                }

                var checkDailyLimitApt = await _configRepo.GetKeyValueConfig("MaxAppointmentsPerDay");
                if (checkDailyLimitApt <= 0)
                {
                    throw new ArgumentException("Invalid configuration: MaxAppointmentsPerDay must be greater than 0.");
                }

                // Validate the max limit appointment per day
                int safetyCounter = 0;
                int maxLookaheadDays = 365;
                while(true)
                {
                    if (checkHoliday == null)
                    {
                        var getCurrentAptCount = await _aptRepo.GetAppointmentNumberDates(dateAssign);
                        if (getCurrentAptCount < checkDailyLimitApt)
                        {
                            break;
                        }
                    }                  

                    dateAssign = dateAssign.AddDays(1);
                    safetyCounter++;

                    if (safetyCounter > maxLookaheadDays)
                    {
                        throw new ArgumentException("Unable to find available appointment slot within 1 year.");
                    }
                }

                // Generate Appointment Number
                var getAppointmentNumber = await _aptRepo.GetAppointmentNumber(dateAssign);

                // Book Appointment
                var newAppointment = new Appointments
                {
                    CustomerId          = request.CustomerId,
                    AppointmentDate     = dateAssign,
                    AppointmentNumber   = getAppointmentNumber,
                    CreatedAt           = DateTime.UtcNow,
                    Status              = 1,
                    ServiceType         = request.ServiceType,
                    Notes               = request.Notes
                };
                await _aptRepo.AddAppointment(newAppointment);

                return new AppointmentResponseDto
                {
                    Id                  = newAppointment.Id,
                    AppointmentDate     = newAppointment.AppointmentDate.ToString("yyyy-MM-dd"),
                    AppointmentNumber   = newAppointment.AppointmentNumber,
                    CustomerId          = newAppointment.CustomerId,
                    Notes               = newAppointment.Notes,
                    ServiceType         = newAppointment.ServiceType
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while booking the appointment.");
                throw;
            }
        }

        public async Task<List<AppointmentResponseDto>> ViewAppointmentsByDate(string appointmentDate)
        {
            try
            {
                var dateAssign      = DateTime.Parse(appointmentDate);
                var appointments    = new List<AppointmentResponseDto>();
                var aptList         = await _aptRepo.GetAppointmentsByDate(dateAssign);

                foreach (var apt in aptList)
                {
                    appointments.Add(new AppointmentResponseDto
                    {
                        Id = apt.Id,
                        AppointmentDate = apt.AppointmentDate.ToString("yyyy-MM-dd"),
                        AppointmentNumber = apt.AppointmentNumber,
                        CustomerId = apt.CustomerId,
                        Notes = apt.Notes,
                        ServiceType = apt.ServiceType
                    });
                }

                return appointments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving appointments.");
                throw;
            }
        }

        public async Task<bool> UpdateAppointmentStatus(int appointmentId, int status)
        {
            try
            {
                var updatedStatus = await _aptRepo.UpdateAppointmentStatus(appointmentId, status);
                return updatedStatus;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the appointment status.");
                throw;
            }
        }
    }
}
