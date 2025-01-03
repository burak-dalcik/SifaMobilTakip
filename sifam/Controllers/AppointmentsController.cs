using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sifam.Data;
using sifam.DTOs;
using sifam.Models;

namespace sifam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AppointmentController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("Upcoming")]
        public async Task<ActionResult<IEnumerable<AppointmentCreateDto>>> GetUpcomingAppointments()
        {
            var upcomingAppointments = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.AppointmentDate > DateTime.Now)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();

            var appointmentDTOs = _mapper.Map<List<AppointmentCreateDto>>(upcomingAppointments);

            return Ok(appointmentDTOs);
        }

        // GET: api/Appointment/Past
        [HttpGet("Past")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetPastAppointments()
        {
            var pastAppointments = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.AppointmentDate <= DateTime.Now) // Şu anki zamandan önceki randevular
                .OrderByDescending(a => a.AppointmentDate) // Tarihe göre sıralı
                .ToListAsync();

            return Ok(pastAppointments);
        }

        // GET: api/Appointment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointment);
        }

         // POST: api/Appointment
         [HttpPost]
         public IActionResult CreateAppointment([FromBody] AppointmentCreateDto appointmentDto)
         {
             // DTO'yu nesneye dönüştürmek için AutoMapper kullanıyoruz
             var appointment = _mapper.Map<Appointment>(appointmentDto);

             _context.Appointments.Add(appointment);
             _context.SaveChanges();

             return CreatedAtAction(nameof(GetAppointment), new { id = appointment.AppointmentId }, appointment); // 'Id' yerine 'AppointmentId' kullanalım
         }
        


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] AppointmentCreateDto appointmentDto)
        {
            if (id != appointmentDto.AppointmentId)
            {
                return BadRequest("Appointment ID does not match."); // Daha açıklayıcı hata mesajı
            }

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == id);

            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }

            // DTO'dan randevuya veri aktarımı
            appointment.AppointmentDate = appointmentDto.AppointmentDate;
            appointment.Description = appointmentDto.Description;
            appointment.DoctorId = appointmentDto.DoctorId;
            appointment.PatientId = appointmentDto.PatientId;

            _context.Entry(appointment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
                {
                    return NotFound("Appointment not found during update.");
                }
                else
                {
                    throw;
                }
            }

            return Ok(appointment); // Güncellenen randevu objesini dönebilirsiniz.
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAppointment(int id)
        {
            var appointment = _context.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound(new { Message = "Appointment not found." });
            }

            _context.Appointments.Remove(appointment);
            _context.SaveChanges();

            return NoContent(); // Başarılı işlem için 204 döner
        }


        // Yardımcı fonksiyon
        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentId == id);
        }



    }
}
