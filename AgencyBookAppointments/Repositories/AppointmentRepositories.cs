using AgencyBookAppointments.Data;
using Microsoft.EntityFrameworkCore;

namespace AgencyBookAppointments.Repositories
{
    public interface IAppointmentRepositories
    {
        Task<int> GetAppointmentNumberDates(DateTime date);
        Task<bool> AddAppointment(Models.Appointments appointment);
        Task<string> GetAppointmentNumber(DateTime date);
        Task<List<Models.Appointments>> GetAppointmentsByDate(DateTime date);
        Task<bool> UpdateAppointmentStatus(int appointmentId, int status);
    }

    public class AppointmentRepositories : IAppointmentRepositories
    {
        private readonly ApplicationDbContext _context;
        public AppointmentRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetAppointmentNumberDates(DateTime date)
        {
            try
            {
                var appointmentCount = await _context.Appointments.CountAsync(a => a.AppointmentDate.Date == date.Date && a.Status == 1);
                return appointmentCount;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Models.Appointments>> GetAppointmentsByDate(DateTime date)
        {
            try
            {
                var appointments = await _context.Appointments
                    .Include(a => a.Customer)
                    .Where(a => a.AppointmentDate.Date == date.Date)
                    .ToListAsync();

                return appointments;
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> GetAppointmentNumber(DateTime date)
        {
            var dateKey = date.Date;
            int newNumber;

            using var trx = await _context.Database.BeginTransactionAsync();

            try
            {
                var counter = await _context.AppointmentCounters.FirstOrDefaultAsync(c => c.DateKey == dateKey);

                if (counter == null)
                {
                    counter = new Models.AppointmentCounters { DateKey = dateKey, LastNumber = 0 };
                    _context.AppointmentCounters.Add(counter);
                }

                counter.LastNumber++;
                newNumber = counter.LastNumber;

                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }
            catch
            {
                await trx.RollbackAsync();
                throw;
            }

            return $"Q{date:yyyyMMdd}-{newNumber:D3}";
        }

        public async Task<bool> AddAppointment(Models.Appointments appointment)
        {
            try
            {
                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateAppointmentStatus(int appointmentId, int status)
        {
            try
            {
                var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == appointmentId);
                if(appointment == null)
                {
                    throw new ArgumentException("Appointment not found.");
                }

                appointment.Status = status;
                appointment.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
