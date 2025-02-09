using CareerSpotlightApi.Models;
using CareerSpotlightBase.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CareerSpotlightApi.Data
{
    public class CareerSpotlightContext : IdentityDbContext<User>
    {
        public CareerSpotlightContext(DbContextOptions<CareerSpotlightContext> options) : base(options) { }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Profile>().ToTable("profiles");
            modelBuilder.Entity<Education>().ToTable("educations");
            modelBuilder.Entity<Experience>().ToTable("experiences");
        }
    }
}
