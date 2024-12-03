using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sifam.Data;
using sifam.Models;

namespace sifam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FamilyMembersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FamilyMembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/FamilyMembers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FamilyMember>>> GetFamilyMembers()
        {
            var familyMembers = await _context.FamilyMembers
                .Include(f => f.Patient)
                .Include(f => f.User)
                .ToListAsync();

            return Ok(familyMembers);
        }

        // GET: api/FamilyMembers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FamilyMember>> GetFamilyMember(int id)
        {
            var familyMember = await _context.FamilyMembers
                .Include(f => f.Patient)
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.FamilyMemberId == id);

            if (familyMember == null)
            {
                return NotFound();
            }

            return Ok(familyMember);
        }

        // POST: api/FamilyMembers
        // POST: api/FamilyMembers
        [HttpPost]
        public async Task<ActionResult<FamilyMember>> CreateFamilyMember([FromBody] FamilyMember familyMember)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Veritabanından User ve Patient kontrolü
            var user = await _context.Users.FindAsync(familyMember.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var patient = await _context.Patients.FindAsync(familyMember.PatientId);
            if (patient == null)
            {
                return NotFound("Patient not found.");
            }

            // User ve Patient nesnelerinin zaten var olduğuna emin olduktan sonra
            // sadece ID'lerini kullanarak FamilyMember oluşturulmalı.

            familyMember.User = user; // Referans olarak ilişkilendir
            familyMember.Patient = patient; // Referans olarak ilişkilendir

            // FamilyMember ekle
            _context.FamilyMembers.Add(familyMember);
            await _context.SaveChangesAsync();

            // FamilyMember başarılı bir şekilde eklendikten sonra döndürülen sonucu belirtmek
            return CreatedAtAction(nameof(GetFamilyMember), new { id = familyMember.FamilyMemberId }, familyMember);
        }



        // PUT: api/FamilyMembers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFamilyMember(int id, FamilyMember familyMember)
        {
            if (id != familyMember.FamilyMemberId)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(familyMember).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FamilyMemberExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/FamilyMembers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFamilyMember(int id)
        {
            var familyMember = await _context.FamilyMembers.FindAsync(id);

            if (familyMember == null)
            {
                return NotFound();
            }

            _context.FamilyMembers.Remove(familyMember);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FamilyMemberExists(int id)
        {
            return _context.FamilyMembers.Any(e => e.FamilyMemberId == id);
        }
    }
}
