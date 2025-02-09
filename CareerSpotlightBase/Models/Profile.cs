using System.ComponentModel.DataAnnotations;
using System.Net;

namespace CareerSpotlightBase.Models
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }  // Primary key
        public string Name { get; set; }
        public string Email { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Bio { get; set; }
        public List<Education> Education { get; set; }
        public List<Experience> Experience { get; set; }
        public List<string> Skills { get; set; }

        public Profile(string name, string email, DateOnly dateOfBirth, string address, string phoneNumber, string bio)
        {
            Name = name;
            Email = email;
            DateOfBirth = dateOfBirth;
            Address = address;
            PhoneNumber = phoneNumber;
            Bio = bio;
            Education = new List<Education>();
            Experience = new List<Experience>();
            Skills = new List<string>();
        }
    }
}
