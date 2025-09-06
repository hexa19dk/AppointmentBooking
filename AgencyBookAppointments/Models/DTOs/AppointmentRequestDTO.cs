namespace AgencyBookAppointments.Models.DTOs
{
    public class AppointmentRequestDTO
    {
        public string? AppointmentDate { get; set; }
        public int CustomerId { get; set; }
        public string Notes { get; set; } = string.Empty;
        public int ServiceType { get; set; }
    }

    public class AppointmentResponseDto
    {
        public int Id { get; set; }
        public string? AppointmentDate { get; set; }
        public string? AppointmentNumber { get; set; }
        public int CustomerId { get; set; }
        public string Notes { get; set; } = string.Empty;
        public int ServiceType { get; set; }
    }

   

}
