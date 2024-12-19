using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sifam.Data;
using sifam.Models;
using sifam.Validators;
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
                return NotFound("Kayıt bulunamadı.");
            }

            return Ok(caregiver);
        }

        [HttpPost]
        public async Task<ActionResult<Caregiver>> CreateCaregiver([FromBody] Caregiver caregiver)
        {
            var errors = CaregiverValidator.Validate(caregiver);
            if (errors.Any())
            {
                return BadRequest(string.Join(", ", errors)); // Hataları döndür
            }

            // Kullanıcı ve Hasta kontrolü
            var userExists = await _context.Users.AnyAsync(u => u.UserId == caregiver.UserId);
            var patientExists = await _context.Patients.AnyAsync(p => p.PatientId == caregiver.AssignedPatientId);

            if (!userExists)
            {
                return BadRequest("Belirtilen kullanıcı bulunamadı.");
            }

            if (!patientExists)
            {
                return BadRequest("Belirtilen hasta bulunamadı.");
            }

            _context.Caregivers.Add(caregiver);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCaregiver), new { id = caregiver.CaregiverId }, caregiver);
        }


        // PUT: api/Caregivers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCaregiver(int id, [FromBody] Caregiver caregiver)
        {
            if (id != caregiver.CaregiverId)
            {
                return BadRequest("Caregiver ID eşleşmiyor.");
            }

            var patientExists = await _context.Patients.AnyAsync(p => p.PatientId == caregiver.AssignedPatientId);

            if (!patientExists)
            {
                return BadRequest("Bağlı Hasta bulunamadı.");
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
            var caregiver = await _context.Caregivers
                .FirstOrDefaultAsync(c => c.CaregiverId == id);

            if (caregiver == null)
            {
                return NotFound("Caregiver bulunamadı.");
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
