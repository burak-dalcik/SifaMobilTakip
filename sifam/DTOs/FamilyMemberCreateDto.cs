namespace sifam.DTOs
{
    public class FamilyMemberCreateDto
    {
        public int UserId { get; set; }
        public int PatientId { get; set; }
        public string Relationship { get; set; }
    }
}
