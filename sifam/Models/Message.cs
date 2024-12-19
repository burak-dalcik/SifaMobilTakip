using System.ComponentModel.DataAnnotations;

namespace sifam.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        [Required]
        public int SenderId { get; set; } // Gönderen kullanıcı

        [Required]
        public string MessageContent { get; set; } // Mesaj içeriği

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.Now; // Gönderim zamanı

        public string MessageType { get; set; } // Örn: "Doktor-Hasta Yakını"

        public bool IsRead { get; set; } = false; 

        public ICollection<MessageParticipant> Participants { get; set; }
    }
}
