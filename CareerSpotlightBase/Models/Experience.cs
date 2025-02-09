using System.ComponentModel.DataAnnotations;

namespace CareerSpotlightBase.Models
{
    public class Experience
    {
        [Key]
        public int Id { get; set; } // Primary key
        public required string Company { get; set; }
        public required string Role { get; set; }
        public required string Years { get; set; }
    }
}
