namespace AgencyBookAppointments.Models.DTOs
{
    public class AgencyRequestDTO
    {
    }

    public class CustomerRequestDTO
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
