📅 **Appointment Booking API**
A .NET Core Web API that allows an Agency to manage customer appointments, issue tokens (appointment numbers), and view daily queues.
It supports off days, maximum daily appointments, and automatically shifts excess bookings to the next available day.

🚀 **Features**
- Book customer appointments with token/appointment number.
- Agency can specify holidays and maximum appointments per day.
- Queue visualization: see all customers booked for a given day.
- Structured API response with success/error handling.
- Clean architecture:
  - Business Layer (Services)
  - Data Layer (Repositories, EF Core)
  - Web Layer (Controllers, API)
- Swagger for API documentation.
- Ready for Dependency Injection (DI/IoC).
- Unit Tests with xUnit + Moq.
- GitHub for source control & collaboration.

⚙️ **Setup Instructions**
1. Clone the repository
    git clone https://github.com/<your-username>/AppointmentBooking.git
    cd AppointmentBooking

2. Setup database
    Update appsettings.json with your SQL Server connection string:    
    "ConnectionStrings": {
      "DefaultConnection": "Server=.;Database=AppointmentDb;Trusted_Connection=True;TrustServerCertificate=True;"
    }

3. Run migrations
    cd AgencyBookAppointments
    dotnet ef database update

4. Run the API
    dotnet run --project AgencyBookAppointments
