using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace sifam.Models
{
    public class CaregiverRequest
    {
        public int RequestId { get; set; } 
        public int CaregiverId { get; set; } 
        public string RequestText { get; set; }
        public DateTime RequestDate { get; set; } 
        public bool IsResolved { get; set; } 
        public Caregiver? Caregiver { get; set; } 
    }
}



