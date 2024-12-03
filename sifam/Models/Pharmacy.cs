using System.ComponentModel.DataAnnotations;

namespace sifam.Models
{
    public class Pharmacy
    {
        [Key]
        public int PharmacyId { get; set; } 

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } 

        [Required]
        [MaxLength(255)]
        public string Location { get; set; } 

        public string AvailableMedications { get; set; }
    }
}
