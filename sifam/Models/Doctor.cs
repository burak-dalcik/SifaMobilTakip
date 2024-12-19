namespace sifam.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; } 
        public int UserId { get; set; } 
        public string Specialization { get; set; } 
        public User User { get; set; }
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    }

}
