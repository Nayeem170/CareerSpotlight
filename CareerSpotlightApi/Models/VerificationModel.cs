using System.ComponentModel.DataAnnotations;

namespace CareerSpotlightApi.Models
{
    public class VerificationModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
