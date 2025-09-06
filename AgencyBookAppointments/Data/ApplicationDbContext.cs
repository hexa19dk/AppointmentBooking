using AgencyBookAppointments.Models;
using Microsoft.EntityFrameworkCore;

namespace AgencyBookAppointments.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<Models.Customers> Customers { get; set; }
        public DbSet<Models.Appointments> Appointments { get; set; }
        public DbSet<Models.Holiday> Holidays { get; set; }
        public DbSet<Models.Configuration> Configurations { get; set; }
        public DbSet<Models.AppointmentCounters> AppointmentCounters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Appointments>()
                .HasOne(a => a.Customer)
                .WithMany(c => c.Appoinments)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Appointments>()
                .HasIndex(a => new { a.AppointmentDate, a.AppointmentNumber })
                .IsUnique();

            modelBuilder.Entity<Holiday>()
                .HasIndex(h => h.HolidayDate)
                .IsUnique();

            modelBuilder.Entity<Configuration>()
               .HasIndex(c => c.KeyName)
               .IsUnique();
        }
    }
}
