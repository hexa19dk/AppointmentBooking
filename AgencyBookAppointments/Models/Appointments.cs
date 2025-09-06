namespace AgencyBookAppointments.Models
{
    public class Appointments
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentNumber { get; set; } = string.Empty;
        public int ServiceType { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int Status { get; set; } = 1;

        public Customers? Customer { get; set; }
    }
}
