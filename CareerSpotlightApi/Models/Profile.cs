using System.ComponentModel.DataAnnotations;

namespace CareerSpotlightApi.Models
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }  // Primary key
        public string Name { get; set; }
        public string Bio { get; set; }
        public List<Education> Education { get; set; }
        public List<Experience> Experience { get; set; }
        public List<string> Skills { get; set; }

        public Profile(string name, string bio)
        {
            Name = name;
            Bio = bio;
            Education = new List<Education>();
            Experience = new List<Experience>();
            Skills = new List<string>();
        }
    }
}
