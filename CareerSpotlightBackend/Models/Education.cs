namespace CareerSpotlightBackend.Models
{
    public class Education
    {
        public int Id { get; set; } // Primary key
        public required string Institution { get; set; }
        public required string Degree { get; set; }
        public int Year { get; set; }
    }
}
