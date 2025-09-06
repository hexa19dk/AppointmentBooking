namespace AgencyBookAppointments.Models
{
    public class Enum
    {
    }

    public enum AppointmentStatus
    {
        Active = 1,
        Cancelled = 2,
        Completed = 3,
        Pending = 4
    }

    public enum ServiceType
    {
        Pickup = 1,
        Delivery = 2,
        ExpressDelivery = 3,
        Return = 4,
        Warehousing = 5
    }
}
