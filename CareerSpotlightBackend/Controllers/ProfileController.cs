using CareerSpotlightBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace CareerSpotlightBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly Profile _profile;

        public ProfileController()
        {
            _profile = new Profile(
                name: "Jane Doe",
                bio: "Passionate software engineer with a love for developing innovative programs."
            )
            {
                Education = new List<Education>
                {
                    new Education
                    {
                        Institution = "Tech University",
                        Degree = "B.Sc. in Computer Science",
                        Year = 2020
                    }
                },
                Experience = new List<Experience>
                {
                    new Experience
                    {
                        Company = "Innovatech Solutions",
                        Role = "Software Developer",
                        Years = "2020-2023"
                    }
                },
                Skills = new List<string> { "C#", ".NET", "REST APIs", "SQL", "JavaScript" }
            };
        }

        [HttpGet]
        public ActionResult<Profile> GetProfile()
        {
            return Ok(_profile);
        }
    }
}
