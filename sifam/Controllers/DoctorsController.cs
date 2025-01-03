using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sifam.Data;
using sifam.Models;
using sifam.DTOs;


namespace sifam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]  // API controller olduğunu belirtir
    public class DoctorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Doctors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors()
        {
            var doctors = await _context.Doctors
                .Include(d => d.User)
                .ToListAsync();
            return Ok(doctors);  // JSON formatında dönüş yapar
        }

        // GET: api/Doctors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetDoctor(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.DoctorId == id);

            if (doctor == null)
            {
                return NotFound();  // Hata: 404
            }

            return Ok(doctor);  // JSON formatında dönüş yapar
        }

        [HttpPost]
        public async Task<ActionResult<Doctor>> CreateDoctor([FromBody] DoctorDTO doctorDto)
        {
            // Kullanıcı doğrulama
            var userExists = await _context.Users.AnyAsync(u => u.UserId == doctorDto.DoctorId);
            if (!userExists)
            {
                return BadRequest("Belirtilen kullanıcı bulunamadı.");
            }

            // Yeni doctor nesnesini oluştur
            var doctor = new Doctor
            {
                UserId = doctorDto.DoctorId,
                Specialization = doctorDto.Specialization
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.DoctorId }, doctor);
        }








        // PUT: api/Doctors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] Doctor doctor)
        {
            if (id != doctor.DoctorId)
            {
                return BadRequest("ID mismatch");  // Hata: 400
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Hata: 400
            }

            try
            {
                _context.Update(doctor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
                {
                    return NotFound();  // Hata: 404
                }
                throw;
            }

            return NoContent();  // Başarılı bir güncelleme: 204
        }

        // DELETE: api/Doctors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();  // Hata: 404
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return NoContent();  // Başarılı silme işlemi: 204
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.DoctorId == id);
        }
    }
}
