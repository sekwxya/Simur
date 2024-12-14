using System.CodeDom;
using System.Security.Cryptography.X509Certificates;
using TourAgency.Models;
using Microsoft.EntityFrameworkCore;


namespace TourAgency.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<CountryStatistics> CountryStatistics { get; set; }
        public DbSet<Discount> Discount { get; set; }
        public DbSet<Review> Rewiew { get; set; }
        public DbSet<Tour> Tour { get; set; }
        public DbSet<TourPlan> TourPlan { get; set; }
        public DbSet<TourRequest> TourRequest { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<TourHistory> TourHistory { get; set; }
        
    }
}