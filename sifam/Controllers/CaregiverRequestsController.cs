using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sifam.Data;
using sifam.Models;

namespace sifam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaregiverRequestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CaregiverRequestController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CaregiverRequest
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CaregiverRequest>>> GetCaregiverRequests()
        {
            var caregiverRequests = await _context.CaregiverRequests.ToListAsync();

            if (caregiverRequests == null || caregiverRequests.Count == 0)
            {
                return NoContent();
            }

            return Ok(caregiverRequests);
        }

        // GET: api/CaregiverRequest/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CaregiverRequest>> GetCaregiverRequest(int id)
        {
            var caregiverRequest = await _context.CaregiverRequests
                .FirstOrDefaultAsync(cr => cr.RequestId == id);

            if (caregiverRequest == null)
            {
                return NotFound();
            }

            return Ok(caregiverRequest);
        }

        // POST: api/CaregiverRequest
        [HttpPost]
        public async Task<ActionResult<CaregiverRequest>> PostCaregiverRequest(CaregiverRequest caregiverRequest)
        {
            // Gelen istek parametrelerini doğrulama
            if (caregiverRequest == null || caregiverRequest.CaregiverId <= 0 || string.IsNullOrEmpty(caregiverRequest.RequestText))
            {
                return BadRequest("CaregiverId ve RequestText gerekli alanlardır.");
            }

            // CaregiverId'nin geçerli bir Caregiver'a ait olup olmadığını kontrol et
            var caregiverExists = await _context.Caregivers.AnyAsync(c => c.CaregiverId == caregiverRequest.CaregiverId);

            if (!caregiverExists)
            {
                return BadRequest("Geçerli bir Caregiver bulunamadı.");
            }

            // Veritabanına yeni CaregiverRequest ekleme
            _context.CaregiverRequests.Add(caregiverRequest);
            await _context.SaveChangesAsync();

            // Yeni kaydın ID'sini içeren bir yanıt dönme
            return CreatedAtAction(nameof(GetCaregiverRequest), new { id = caregiverRequest.RequestId }, caregiverRequest);
        }

    }
}
