namespace sifam.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public int UserId { get; set; }
        public string TcNo { get; set; }
        public string Disease { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }

        public User User { get; set; }
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    }

}
