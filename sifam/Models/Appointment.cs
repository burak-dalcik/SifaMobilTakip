namespace sifam.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; } // Primary Key
        public DateTime AppointmentDate { get; set; }
        public string Description { get; set; } // Randevu açıklaması

        // Doctor ile ilişki
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        // Patient ile ilişki
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
