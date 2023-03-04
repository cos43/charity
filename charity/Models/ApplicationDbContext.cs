using Microsoft.EntityFrameworkCore;

namespace charity.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserModel>().HasData(
            new UserModel
            {
                Id = 1, Email = "admin@admin", PhoneNumber = "1234567890", FirstName = "admin", LastName = "admin",
                Password = UserModel.ComputeMD5Hash("admin"),
                Role = UserRole.Admin, Address = "Earth"
            }
        );
    }

    public DbSet<UserModel> Users { get; set; }
    public DbSet<ApplicationModel> Applications { get; set; }
    public DbSet<DonationModel> Donations { get; set; }
    public DbSet<EventsModel> Events { get; set; }
    public DbSet<UserEventsModel> UserEvents { get; set; }
}