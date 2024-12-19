using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using sifam.Data;
using sifam.Models;

namespace sifam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Appointment/Upcoming
        [HttpGet("Upcoming")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetUpcomingAppointments()
        {
            var upcomingAppointments = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.AppointmentDate > DateTime.Now) // Şu anki zamandan sonraki randevular
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();

            return Ok(upcomingAppointments);
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

        // POST: api/Appointment
        [HttpPost]
        public async Task<ActionResult<Appointment>> CreateAppointment(Appointment appointment)
        {
            if (appointment.AppointmentDate <= DateTime.Now)
            {
                return BadRequest("Appointment date must be in the future.");
            }

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUpcomingAppointments), new { id = appointment.AppointmentId }, appointment);
        }
    }
}