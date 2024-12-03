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
    public class PatientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            var patients = await _context.Patients.Include(p => p.User).ToListAsync();
            return Ok(patients); 
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            var patient = await _context.Patients.Include(p => p.User)
                                                  .FirstOrDefaultAsync(p => p.PatientId == id);

            if (patient == null)
            {
                return NotFound();  
            }

            return Ok(patient);  
        }

        
        [HttpPost]
        public async Task<IActionResult> PostPatient(Patient patient)
        {
            
            var user = await _context.Users.FindAsync(patient.UserId);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            
            patient.User = user;

            
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPatient", new { id = patient.PatientId }, patient);
        }



        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, Patient patient)
        {
            if (id != patient.PatientId)
            {
                return BadRequest("Patient ID mismatch");  
            }

            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();  
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();  
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.PatientId == id);
        }
    }
}
