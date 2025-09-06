using Microsoft.EntityFrameworkCore;

namespace AgencyBookAppointments.Models
{
    public class Configuration
    {
        public int Id { get; set; }
        public string KeyName { get; set; } = string.Empty;
        public string KeyValue { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
