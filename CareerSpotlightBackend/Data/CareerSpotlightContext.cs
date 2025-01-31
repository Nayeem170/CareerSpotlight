﻿using CareerSpotlightBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CareerSpotlightBackend.Data
{
    public class CareerSpotlightContext : DbContext
    {
        public CareerSpotlightContext(DbContextOptions<CareerSpotlightContext> options) : base(options) { }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
    }
}
