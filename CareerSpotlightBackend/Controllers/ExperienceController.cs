using CareerSpotlightBackend.Data;
using CareerSpotlightBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareerSpotlightBackend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExperienceController : ControllerBase
    {
        private readonly CareerSpotlightContext _context;

        public ExperienceController(CareerSpotlightContext context)
        {
            _context = context;
        }

        // GET: api/Experience
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Experience>>> GetExperiences()
        {
            return await _context.Experiences.ToListAsync();
        }

        // GET: api/Experience/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Experience>> GetExperience(int id)
        {
            var experience = await _context.Experiences.FindAsync(id);

            if (experience == null)
            {
                return NotFound();
            }

            return experience;
        }

        // POST: api/Experience
        [HttpPost]
        public async Task<ActionResult<Experience>> PostExperience(Experience experience)
        {
            _context.Experiences.Add(experience);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetExperience), new { id = experience.Id }, experience);
        }

        // PUT: api/Experience/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExperience(int id, Experience experience)
        {
            if (id != experience.Id)
            {
                return BadRequest();
            }

            _context.Entry(experience).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExperienceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Experience/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExperience(int id)
        {
            var experience = await _context.Experiences.FindAsync(id);
            if (experience == null)
            {
                return NotFound();
            }

            _context.Experiences.Remove(experience);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExperienceExists(int id)
        {
            return _context.Experiences.Any(e => e.Id == id);
        }
    }
}
