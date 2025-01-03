namespace sifam.DTOs
{
    public class DoctorDTO
    {
        public int DoctorId { get; set; }
        public string Specialization { get; set; }
        // Doctor ile ilişkili Appointment bilgilerini dışarıda bırakıyoruz.
    }
}
