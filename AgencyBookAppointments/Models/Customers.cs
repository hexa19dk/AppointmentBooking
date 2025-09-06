namespace AgencyBookAppointments.Models
{
    public class Customers
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Appointments> Appoinments { get; set; } = new List<Appointments>();
    }
}
