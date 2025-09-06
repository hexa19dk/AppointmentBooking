using System.ComponentModel.DataAnnotations;

namespace AgencyBookAppointments.Models
{
    public class AppointmentCounters
    {
        [Key]
        public DateTime DateKey { get; set; }
        public int LastNumber { get; set; }
    }
}
