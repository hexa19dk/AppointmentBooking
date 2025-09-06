using AgencyBookAppointments.Data;
using AgencyBookAppointments.Models;
using Microsoft.EntityFrameworkCore;

namespace AgencyBookAppointments.Repositories
{
    public interface IConfigurationRepositories
    {
        Task<int> GetKeyValueConfig(string valueName);
        Task<bool> CreateNewKey(string keyName, string keyValue);
        Task<bool> DeleteKey(string keyName);
        Task<bool> UpdateKey(string keyName, string keyValue);
        Task<bool> UpsertConfig(string keyName, string keyValue);
    }

    public class ConfigurationRepositories : IConfigurationRepositories
    {
        private readonly ApplicationDbContext _context;
        public ConfigurationRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetKeyValueConfig(string valueName)
        {
            try
            {
                var getKeyValue = await _context.Configurations.FirstOrDefaultAsync(c => c.KeyName == valueName);
                return int.TryParse(getKeyValue?.KeyValue, out var value) ? value : 0;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpsertConfig(string keyName, string keyValue)
        {
            try
            {
                var configItem = await _context.Configurations.FirstOrDefaultAsync(c => c.KeyName == keyName);

                if (configItem == null)
                {
                    var newConfig = new Configuration
                    {
                        KeyName   = keyName,
                        KeyValue  = keyValue,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.Configurations.Add(newConfig);
                }
                else
                {
                    configItem.KeyName = keyName;
                    configItem.KeyValue = keyValue;
                    configItem.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> CreateNewKey(string keyName, string keyValue)
        {
            try
            {
                var config = new Models.Configuration
                {
                    KeyName   = keyName,
                    KeyValue  = keyValue,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Configurations.Add(config);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteKey(string keyName)
        {
            try
            {
                var config = _context.Configurations.FirstOrDefault(c => c.KeyName == keyName);
                if (config != null)
                {
                    _context.Configurations.Remove(config);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch
            {
                throw;
            }
        }        

        public async Task<bool> UpdateKey(string keyName, string keyValue)
        {
            try
            {
                var config = _context.Configurations.FirstOrDefault(c => c.KeyName == keyName);
                if (config != null)
                {
                    config.KeyValue = keyValue;
                    config.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch
            {
                throw;
            }
        }
    }
}
