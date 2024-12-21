using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public AppointmentsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


       /*  public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }
       */

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
        public async Task<IActionResult> PostAppointment(AppointmentDTO appointmentDto)
        {
            // Check if the related Doctor and Patient exist
            var doctor = await _context.Doctors.FindAsync(appointmentDto.DoctorId);
            var patient = await _context.Patients.FindAsync(appointmentDto.PatientId);

            if (doctor == null || patient == null)
            {
                return BadRequest("Doctor or Patient not found");
            }

            // Map DTO to Appointment Model
            var appointment = _mapper.Map<Appointment>(appointmentDto);

            // Add to database
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            // Return created response
            return CreatedAtAction("GetAppointment", new { id = appointment.AppointmentId }, appointment);
        }


    }
}