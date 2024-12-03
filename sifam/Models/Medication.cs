using System.ComponentModel.DataAnnotations;

namespace sifam.Models
{
    public class Medication
    {
        [Key]
        public int MedicationId { get; set; } 

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } 

        public string Description { get; set; } 

        public string SideEffects { get; set; } 
    }
}
