using CareerSpotlightBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace CareerSpotlightBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EducationController : ControllerBase
    {
        private readonly List<Education> _education;

        public EducationController()
        {
            _education = new List<Education>
            {
                new Education
                {
                    Institution = "Tech University",
                    Degree = "B.Sc. in Computer Science",
                    Year = 2020
                }
            };
        }

        [HttpGet]
        public ActionResult<IEnumerable<Education>> GetEducation()
        {
            return Ok(_education);
        }

    }
}