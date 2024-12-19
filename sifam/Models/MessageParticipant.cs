using System.ComponentModel.DataAnnotations;

namespace sifam.Models
{
    public class MessageParticipant
    {
        [Key]
        public int ParticipantId { get; set; }

        [Required]
        public int MessageId { get; set; } // Mesajın ID'si (Foreign Key)

        [Required]
        public int UserId { get; set; } // Katılımcı Kullanıcı ID'si (Foreign Key)

        [Required]
        public string Role { get; set; } // Örn: "Doktor", "Hasta Yakını", "Bakıcı"

        // Navigation Properties
        public Message Message { get; set; }
        public User User { get; set; }

    }
}
