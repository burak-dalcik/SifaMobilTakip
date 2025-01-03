namespace sifam.DTOs
{
    public class AppointmentCreateDto
    {
        public int AppointmentId { get; set; } // Bu ekleniyor, güncelleme işlemleri için
        public DateTime AppointmentDate { get; set; }
        public string Description { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
    }
}
