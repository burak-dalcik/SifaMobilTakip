using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sifam.Data;
using sifam.DTOs;
using sifam.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                return NotFound("FamilyMember bulunamadı.");
            }

            return Ok(familyMember);
        }

        // POST: api/FamilyMembers
        [HttpPost]
        public async Task<ActionResult<FamilyMember>> CreateFamilyMember([FromBody] FamilyMemberCreateDto familyMemberDto)
        {
            // Kullanıcı ve Hasta kontrolü
            var userExists = await _context.Users.AnyAsync(u => u.UserId == familyMemberDto.UserId);
            var patientExists = await _context.Patients.AnyAsync(p => p.PatientId == familyMemberDto.PatientId);

            if (!userExists)
            {
                return BadRequest("Belirtilen kullanıcı bulunamadı.");
            }

            if (!patientExists)
            {
                return BadRequest("Belirtilen hasta bulunamadı.");
            }

            // Yeni FamilyMember nesnesini oluştur
            var familyMember = new FamilyMember
            {
                UserId = familyMemberDto.UserId,
                PatientId = familyMemberDto.PatientId,
                Relationship = familyMemberDto.Relationship
            };

            _context.FamilyMembers.Add(familyMember);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFamilyMember), new { id = familyMember.FamilyMemberId }, familyMember);
        }

        // PUT: api/FamilyMembers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFamilyMember(int id, [FromBody] FamilyMember familyMember)
        {
            if (id != familyMember.FamilyMemberId)
            {
                return BadRequest("FamilyMember ID eşleşmiyor.");
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
                    return NotFound("FamilyMember bulunamadı.");
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
                return NotFound("FamilyMember bulunamadı.");
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
