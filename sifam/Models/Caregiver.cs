namespace sifam.Models
{
    public class Caregiver
    {
        public int CaregiverId { get; set; } 
        public int UserId { get; set; } 
        public int AssignedPatientId { get; set; } 
        public User? User { get; set; }
        public Patient? AssignedPatient { get; set; } 
        public ICollection<CaregiverRequest>? CaregiverRequests { get; set; } 
    }
}
