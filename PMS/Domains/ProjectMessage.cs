using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS.Domains
{

    [Table("ProjectMessage")]
    public class ProjectMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Reciever { get; set; }   // User who receives message

        public string FromMessage { get; set; } // Sender user id

        [MaxLength(2000)]
        public string MessageInfo { get; set; }

        [MaxLength(200)]
        public string MessageStatus { get; set; } // e.g., Sent / Seen / Delivered

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
