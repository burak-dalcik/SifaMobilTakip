namespace sifam.DTOs
{
    public class PatientDTO
    {
        public int PatientId { get; set; }
        public string TcNo { get; set; }
        public string Disease { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }
}
