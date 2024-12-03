using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sifam.Models
{
    public class FamilyMember
    {
        [Key]
        public int FamilyMemberId { get; set; } 

        [Required]
        public int UserId { get; set; } 

        [Required]
        public int PatientId { get; set; } 

        [Required]
        [MaxLength(50)]
        public string Relationship { get; set; } 

        [ForeignKey("UserId")]
        public User User { get; set; } 

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } 
    }
}
