namespace sifam.DTOs
{
    public class AppointmentDTO
    {
        public DateTime AppointmentDate { get; set; }
        public string Description { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
    }
}
