using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sifam.Models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; } 

        [Required]
        public int DoctorId { get; set; } 

        [Required]
        public int PatientId { get; set; } 

        [Required]
        public DateTime PrescriptionDate { get; set; } 

        public string MedicationList { get; set; } 

        public string Notes { get; set; } 

        [Required]
        [MaxLength(11)]
        public string RecipientTcNo { get; set; } 

        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; } 

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } 
    }
}
