using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sifam.Data;
using sifam.DTOs;
using sifam.Models;

namespace sifam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PatientsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDTO>>> GetPatients()
        {
            var patients = await _context.Patients.Include(p => p.User).ToListAsync();
            var patientDTOs = _mapper.Map<List<PatientDTO>>(patients);
            return Ok(patientDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDTO>> GetPatient(int id)
        {
            var patient = await _context.Patients.Include(p => p.User)
                                                  .FirstOrDefaultAsync(p => p.PatientId == id);

            if (patient == null)
            {
                return NotFound();
            }

            var patientDTO = _mapper.Map<PatientDTO>(patient);
            return Ok(patientDTO);
        }

        [HttpPost]
        public async Task<ActionResult<PatientDTO>> PostPatient(PatientDTO patientDTO)
        {
            var patient = _mapper.Map<Patient>(patientDTO);

            var user = await _context.Users.FindAsync(patient.UserId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            patient.User = user;
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            var createdPatientDTO = _mapper.Map<PatientDTO>(patient);
            return CreatedAtAction(nameof(GetPatient), new { id = patient.PatientId }, createdPatientDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, PatientDTO patientDTO)
        {
            if (id != patientDTO.PatientId)
            {
                return BadRequest("ID mismatch.");
            }

            var patient = await _context.Patients.Include(p => p.User).FirstOrDefaultAsync(p => p.PatientId == id);

            if (patient == null)
            {
                return NotFound();
            }

            _mapper.Map(patientDTO, patient);
            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Patients.Any(e => e.PatientId == id))
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
    }
}
