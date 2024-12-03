using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sifam.Data;
using sifam.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sifam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaregiversController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CaregiversController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Caregivers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Caregiver>>> GetCaregivers()
        {
            var caregivers = await _context.Caregivers
                .Include(c => c.AssignedPatient)
                .Include(c => c.User)
                .ToListAsync();

            return Ok(caregivers);
        }

        // GET: api/Caregivers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Caregiver>> GetCaregiver(int id)
        {
            var caregiver = await _context.Caregivers
                .Include(c => c.AssignedPatient)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CaregiverId == id);

            if (caregiver == null)
            {
                return NotFound();
            }

            return Ok(caregiver);
        }

        // POST: api/Caregivers
        [HttpPost]
        public async Task<ActionResult<Caregiver>> CreateCaregiver([FromBody] Caregiver caregiver)
        {
            if (caregiver == null || caregiver.UserId <= 0 || caregiver.AssignedPatientId <= 0)
            {
                return BadRequest("UserId and AssignedPatientId are required.");
            }

            // Validation: Check if UserId and AssignedPatientId exist
            var userExists = await _context.Users.AnyAsync(u => u.UserId == caregiver.UserId);
            var patientExists = await _context.Patients.AnyAsync(p => p.PatientId == caregiver.AssignedPatientId);

            if (!userExists || !patientExists)
            {
                return BadRequest("User or Patient not found.");
            }

            _context.Caregivers.Add(caregiver);
            await _context.SaveChangesAsync();

            // Return the created caregiver with its new ID
            return CreatedAtAction(nameof(GetCaregiver), new { id = caregiver.CaregiverId }, caregiver);
        }


        // PUT: api/Caregivers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCaregiver(int id, [FromBody] Caregiver caregiver)
        {
            if (id != caregiver.CaregiverId)
            {
                return BadRequest("Caregiver ID mismatch.");
            }

            var userExists = await _context.Users.AnyAsync(u => u.UserId == caregiver.UserId);
            var patientExists = await _context.Patients.AnyAsync(p => p.PatientId == caregiver.AssignedPatientId);

            if (!userExists || !patientExists)
            {
                return BadRequest("User or Patient not found.");
            }

            _context.Entry(caregiver).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CaregiverExists(id))
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

        // DELETE: api/Caregivers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCaregiver(int id)
        {
            var caregiver = await _context.Caregivers.FindAsync(id);
            if (caregiver == null)
            {
                return NotFound();
            }

            _context.Caregivers.Remove(caregiver);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CaregiverExists(int id)
        {
            return _context.Caregivers.Any(e => e.CaregiverId == id);
        }
    }
}
