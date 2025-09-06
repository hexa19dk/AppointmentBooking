using AgencyBookAppointments.Data;
using AgencyBookAppointments.Models;
using Microsoft.EntityFrameworkCore;

namespace AgencyBookAppointments.Repositories
{
    public interface IAgencyRepositories
    {
        Task<Customers> AddCustomer(Customers customer);
        Task<Customers?> GetCustomerByEmail(string email);
    }

    public class AgencyRepositories : IAgencyRepositories
    {
        private readonly ApplicationDbContext _ctx;
        public AgencyRepositories(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Customers?> GetCustomerByEmail(string email)
        {
            try
            {
                var customer = await _ctx.Customers.FirstOrDefaultAsync(e => e.Email == email);
                return customer!;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Customers> AddCustomer(Customers customer)
        {
            try
            {
                _ctx.Customers.Add(customer);
                await _ctx.SaveChangesAsync();
                return customer;
            }
            catch
            {
                throw;
            }
        }
    }
}
