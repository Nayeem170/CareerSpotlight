using System.ComponentModel.DataAnnotations;

namespace CareerSpotlightApi.Models
{
    public class Education
    {
        [Key]
        public int Id { get; set; } // Primary key
        public required string Institution { get; set; }
        public required string Degree { get; set; }
        public int Year { get; set; }
    }
}
