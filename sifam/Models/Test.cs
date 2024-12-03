using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sifam.Models
{
    public class Test
    {
        [Key]
        public int TestId { get; set; } 

        [Required]
        public int PatientId { get; set; } 

        public string TestType { get; set; } 

        public string TestResults { get; set; } 

        [Required]
        public DateTime TestDate { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } 
    }
}
