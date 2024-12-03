using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sifam.Models
{
    public class PatientActivity
    {
        [Key]
        public int ActivityId { get; set; } 

        [Required]
        public int PatientId { get; set; } 

        public string VideoPath { get; set; } 

        [Required]
        public DateTime ActivityDate { get; set; } 

        public string Notes { get; set; } 

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } 
    }
}
