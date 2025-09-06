using AgencyBookAppointments.Models;
using AgencyBookAppointments.Models.DTOs;
using AgencyBookAppointments.Repositories;

namespace AgencyBookAppointments.Services
{
    public interface IAgencyService
    {
        Task<bool> CreateCustomerAsync(CustomerRequestDTO request);
        Task<bool> CreateHolidayDate(HolidayRequestDTO request);
        Task<bool> UpdateConfigAsync(string keyName, string keyValue);
    }

    public class AgencyService : IAgencyService
    {
        private readonly ILogger<AgencyService> _logger;
        private readonly IAgencyRepositories _agencyRepo;
        private readonly IHolidayRepositories _holidayRepo;
        private readonly IConfigurationRepositories _configRepo;
        public AgencyService(ILogger<AgencyService> logger, IAgencyRepositories agencyRepo, IHolidayRepositories holidayRepo, IConfigurationRepositories config)
        {
            _logger = logger;
            _agencyRepo = agencyRepo;
            _holidayRepo = holidayRepo;
            _configRepo = config;
        }

        public async Task<bool> CreateCustomerAsync(CustomerRequestDTO request)
        {
            try
            {
                var checkExistingMail = await _agencyRepo.GetCustomerByEmail(request.Email);
                if (checkExistingMail != null)
                {
                    throw new ArgumentException("Email already registered.");
                }

                var newCustomer = new Models.Customers
                {
                    FullName    = request.FullName,
                    Email       = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    CreatedAt   = DateTime.UtcNow
                };

                var createdCustomer = await _agencyRepo.AddCustomer(newCustomer);

                _logger.LogInformation("Customer created with ID {CustomerId}", createdCustomer.Id);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CreateHolidayDate(HolidayRequestDTO request)
        {
            try
            {
                var holidayDateConvert = DateTime.Parse(request.HolidayDate);
                var checkHolidayDatesExists = await _holidayRepo.CheckHolidayDates(holidayDateConvert);
                if (checkHolidayDatesExists != null)
                {
                    throw new ArgumentException("Holiday date already exists.");
                }

                var createdHoliday = await _holidayRepo.AddHoliday(request);


                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateConfigAsync(string keyName, string keyValue)
        {
            try
            {
                var config = await _configRepo.UpsertConfig(keyName, keyValue);
                return config;
            }
            catch
            {
                throw;
            }
        }
    }
}
