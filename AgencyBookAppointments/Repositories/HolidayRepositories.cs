using AgencyBookAppointments.Data;
using AgencyBookAppointments.Models;
using AgencyBookAppointments.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AgencyBookAppointments.Repositories
{
    public interface IHolidayRepositories
    {
        Task<bool> AddHoliday(HolidayRequestDTO request);
        Task<List<Holiday>> GetHoliday();
        Task<Holiday> CheckHolidayDates(DateTime date);
    }

    public class HolidayRepositories : IHolidayRepositories
    {
        private readonly ApplicationDbContext _context;
        public HolidayRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Holiday>> GetHoliday()
        {
            try
            {
                var holidays = await _context.Holidays.ToListAsync();
                return holidays;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Holiday> CheckHolidayDates(DateTime date)
        {
            try
            {
                var holiday = await _context.Holidays.FirstOrDefaultAsync(h => h.HolidayDate.Date == date.Date);
                return holiday!;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> AddHoliday(HolidayRequestDTO request)
        {
            try
            {
                var holiday = new Models.Holiday
                {
                    HolidayDate = DateTime.Parse(request.HolidayDate),
                    Description = request.Description!,
                    CreatedAt   = DateTime.UtcNow
                };

                _context.Holidays.Add(holiday);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
