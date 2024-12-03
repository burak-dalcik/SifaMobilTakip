namespace sifam.Models
{
    public class CaregiverRequestViewModel
    {
        public int RequestId { get; set; }
        public int CaregiverId { get; set; }
        public string RequestText { get; set; }
        public DateTime RequestDate { get; set; }
        public bool IsResolved { get; set; }
    }

}
